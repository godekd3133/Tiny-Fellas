using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.GameLift;
using Amazon.Lambda.Core;
using Amazon.GameLift.Model;
using Amazon.Lambda.Serialization.SystemTextJson;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace RequestGameSession
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(Stream input, ILambdaContext context)
        {
            var fleetID = new StreamReader(input, Encoding.UTF8).ReadToEnd();
            var client = new AmazonGameLiftClient("AKIA3MTR52R2BGL7MOGB","ENfwYnCa4B20pg1ro+r1VJDetnOarvEA4DjhGzgv");
            var requestSearchingGameSessionRequest = new SearchGameSessionsRequest();
            requestSearchingGameSessionRequest.FleetId = fleetID;

            var response = await client.SearchGameSessionsAsync(requestSearchingGameSessionRequest);

            GameSession validSession = null;
            foreach (var session in response.GameSessions)
            {
                if (session.CurrentPlayerSessionCount >= session.MaximumPlayerSessionCount) continue;
                else
                {
                    validSession = session;
                    break;
                }
            }

            if (validSession == null)
            {
                //TODO : insteade of making new session instantly, use MatchMaking or Queue
                var creatingGameSessionRequest = new CreateGameSessionRequest();
                creatingGameSessionRequest.FleetId = fleetID;
                creatingGameSessionRequest.GameSessionId = GetNewGameSessionID(response.GameSessions);
                creatingGameSessionRequest.MaximumPlayerSessionCount = 10;
                var creatingGameSessionResponse =await client.CreateGameSessionAsync(creatingGameSessionRequest);
                
                validSession = creatingGameSessionResponse.GameSession;
            }

            var describePlayerSessionRequest = new DescribePlayerSessionsRequest();
            describePlayerSessionRequest.GameSessionId = validSession.GameSessionId;

            var connectPlayerSessions =
                (await client.DescribePlayerSessionsAsync(describePlayerSessionRequest)).PlayerSessions;
            
            var requestingPlaeyerSession = new CreatePlayerSessionRequest();
            requestingPlaeyerSession.PlayerId = GetNewPlayerSessionID(connectPlayerSessions);
            requestingPlaeyerSession.GameSessionId = validSession.GameSessionId;

            var playerSessionResponse = await client.CreatePlayerSessionAsync(requestingPlaeyerSession);
            
            return validSession.IpAddress+"/"+validSession.Port+"/"+playerSessionResponse.PlayerSession.PlayerSessionId;
        }


        private string GetNewGameSessionID(List<GameSession> createdSessions)
        {
            while (true)
            {
                var id = Random.Shared.Next(999999);
                bool isValidID = true;
                foreach (var session in createdSessions)
                {
                    if (int.Parse(session.GameSessionId) == id)
                    {
                        isValidID = false;
                        break;
                    }
                }

                if (isValidID)
                    return id.ToString();
            }
        }
        
        private string GetNewPlayerSessionID(List<PlayerSession> createdSessions)
        {
            while (true)
            {
                var id = Random.Shared.Next(999999);
                bool isValidID = true;
                foreach (var session in createdSessions)
                {
                    if (int.Parse(session.GameSessionId) == id)
                    {
                        isValidID = false;
                        break;
                    }
                }

                if (isValidID)
                    return id.ToString();
            }
        }
    }
}
