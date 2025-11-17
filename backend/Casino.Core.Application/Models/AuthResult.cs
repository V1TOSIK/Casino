namespace Casino.Core.Application.Models
{
    public class AuthResult
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }

        public AuthResult(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
