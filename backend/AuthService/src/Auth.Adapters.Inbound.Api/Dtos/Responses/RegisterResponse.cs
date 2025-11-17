namespace Auth.Adapters.Inbound.Api.Dtos.Responses
{
    public class RegisterResponse
    {
        public string AccessToken { get; }
        public RegisterResponse(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
