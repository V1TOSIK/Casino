namespace Auth.Adapters.Inbound.Api.Dtos.Requests
{
    public class VerifyResetPasswordRequest
    {
        public string VerificationCode { get; set; } = string.Empty;
    }
}
