namespace Auth.Core.Application.Models
{
    public class ForgotPasswordResult
    {
        public string SessionToken { get; }
        public ForgotPasswordResult(string sessionToken)
        {
            SessionToken = sessionToken;
        }
    }
}
