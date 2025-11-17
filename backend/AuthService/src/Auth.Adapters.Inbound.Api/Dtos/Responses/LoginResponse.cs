namespace Auth.Adapters.Inbound.Api.Dtos.Responses
{
    public class LoginResponse
    {
        public LoginResponse(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; }
    }
}