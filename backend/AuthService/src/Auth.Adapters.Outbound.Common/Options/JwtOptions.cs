namespace Auth.Adapters.Outbound.Common.Options
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int AccessTokenExpirationTime { get; set; }
    }
}
