namespace Auth.Adapters.Inbound.Api.Dtos.Responses
{
    public class RestoreResponse
    {
        public string AccessToken { get; }
        public RestoreResponse(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
