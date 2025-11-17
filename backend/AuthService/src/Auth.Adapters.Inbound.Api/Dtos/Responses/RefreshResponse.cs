namespace Auth.Adapters.Inbound.Api.Dtos.Responses
{
    public class RefreshResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public RefreshResponse(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
