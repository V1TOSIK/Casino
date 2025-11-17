namespace Auth.Adapters.Inbound.Api.Dtos.Requests
{
    public class LoginRequest
    {
        public string Credential { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
