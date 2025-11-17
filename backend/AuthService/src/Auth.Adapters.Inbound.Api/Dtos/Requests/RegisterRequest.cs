namespace Auth.Adapters.Inbound.Api.Dtos.Requests
{
    public class RegisterRequest
    {
        public string Credential { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
