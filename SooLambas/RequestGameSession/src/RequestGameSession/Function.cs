using System.Runtime.Serialization.Json;
using System.Buffers;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.GameLift;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.GameLift.Model;
using Amazon.Lambda.Serialization.SystemTextJson;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace RequestGameSession
{
    public class RequestBody
    {
        public string fleetID { get; set; }
    }
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {

            var requestBody = request.Body;
            var jsonBody = JsonSerializer.Deserialize<RequestBody>(requestBody);
            var fleetID = jsonBody.fleetID;
            // var responseJson = string.Format("\"test\": \"{0}\"", fleetID);
            // return new APIGatewayProxyResponse
            // {
            //     StatusCode = 200,
            //     Body = responseJson
            // }; 

            var client = new AmazonGameLiftClient("AKIA3MTR52R2BGL7MOGB","ENfwYnCa4B20pg1ro+r1VJDetnOarvEA4DjhGzgv");
            var requestSearchingGameSessionRequest = new SearchGameSessionsRequest();
            requestSearchingGameSessionRequest.FleetId = fleetID;

            var response = await client.SearchGameSessionsAsync(requestSearchingGameSessionRequest);

            GameSession validSession = null;
            int onActivatingSessionCount = 0;
            foreach (var session in response.GameSessions)
            {
                
                if (session.Status == "ACTIVATING")
                {
                    onActivatingSessionCount++;
                    continue;
                }
                if (session.Status == "ACTIVE" && session.CurrentPlayerSessionCount <= session.MaximumPlayerSessionCount)
                {
                    validSession = session;
                    break;
                }
            }

            if (validSession == null && onActivatingSessionCount == 0)
            {
                //TODO : insteade of making new session instantly, use MatchMaking or Queue
                var creatingGameSessionRequest = new CreateGameSessionRequest();
                creatingGameSessionRequest.FleetId = fleetID;
                creatingGameSessionRequest.GameSessionId = GetNewGameSessionID(response.GameSessions);
                creatingGameSessionRequest.MaximumPlayerSessionCount = 10;
                var creatingGameSessionResponse =await client.CreateGameSessionAsync(creatingGameSessionRequest);
                
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = "{\"body\":\"there was no valids session so i created new one, wait some second and request again please\"}"
                }; 
            }

            var describePlayerSessionRequest = new DescribePlayerSessionsRequest();
            describePlayerSessionRequest.GameSessionId = validSession.GameSessionId;

            var connectPlayerSessions =
                (await client.DescribePlayerSessionsAsync(describePlayerSessionRequest)).PlayerSessions;
            
            var requestingPlaeyerSession = new CreatePlayerSessionRequest();
            requestingPlaeyerSession.PlayerId = GetNewPlayerSessionID(connectPlayerSessions);
            requestingPlaeyerSession.GameSessionId = validSession.GameSessionId;

            var playerSessionResponse = await client.CreatePlayerSessionAsync(requestingPlaeyerSession);
            var returnValueAsJson = string.Format("{\"ip\":\"{0}\", \"port\":{1}, \"player_session_id\":\"{2}\"}",
                validSession.IpAddress
                , validSession.Port
                , playerSessionResponse.PlayerSession.PlayerSessionId);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = returnValueAsJson
            }; 
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
