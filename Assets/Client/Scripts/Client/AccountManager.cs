using System.Collections.Generic;
using Amazon.CognitoIdentityProvider;
using UnityEngine;
using Amazon.CognitoIdentityProvider.Model;
using Cysharp.Threading.Tasks;
using ConfirmSignUpRequest = Amazon.CognitoIdentityProvider.Model.ConfirmSignUpRequest;
using SignUpRequest = Amazon.CognitoIdentityProvider.Model.SignUpRequest;

public class AccountManager : MonoWeakSingleton<AccountManager>
{
    private AmazonCognitoIdentityProviderClient cognitoProvier;
    
    void Awake()
    {
        cognitoProvier = new AmazonCognitoIdentityProviderClient();
    }

    public async UniTask<bool> SignUp(string userName, string email, string pw)
    {
        var signUpRequest = new SignUpRequest()
        {
            ClientId = AWSFleetManager.Instance.Config.UserPoolClientId,
            Username = userName,
            Password = pw,
            UserAttributes = new List<AttributeType>
            {
                new AttributeType
                {
                    Name = "email",
                    Value = email
                }
            }
        };
        var signUpResponse = cognitoProvier.SignUpAsync(signUpRequest);

        if (signUpResponse.IsCompleted)
            return true;
        else
            Debug.Log("Sign up failed with status " + signUpResponse.Status);

        return false;
    }

    public async UniTask<bool> ConfirmSignUp(string userName, string confirmCode)
    {
        var confirmRequest = new ConfirmSignUpRequest()
        {
            ClientId = AWSFleetManager.Instance.Config.UserPoolClientId,
            ConfirmationCode = confirmCode,
            Username = userName,
        };
        var confirmResponse = await cognitoProvier.ConfirmSignUpAsync(confirmRequest);
        if (confirmResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            Debug.Log($"{userName} was confirmed");
            return true;
        }
            return false;
    }
    


    public async UniTask GetAuthentication(string userName, string pw)
    {
        // Set up the authentication request
        var authRequest = new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", userName },
                { "PASSWORD", pw }
            },
            ClientId = userName
        };

        var authResponse = await cognitoProvier.InitiateAuthAsync(authRequest);
    }
}

