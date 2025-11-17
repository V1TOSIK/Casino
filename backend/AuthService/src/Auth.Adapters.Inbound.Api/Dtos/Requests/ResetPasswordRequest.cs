namespace Auth.Adapters.Inbound.Api.Dtos.Requests
{
    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; } = string.Empty;
    }
}
