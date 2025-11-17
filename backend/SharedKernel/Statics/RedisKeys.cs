namespace SharedKernel.Statics
{
    public static class RedisKeys
    {
        public const string ForgotPasswordPrefix = "forgot-password:";
        public const string ForgotSessionPrefix = "forgot-session:";
        public const string ResetTokenPrefix = "reset_token:";

        public static string ForgotPassword(Guid userId) => $"{ForgotPasswordPrefix}{userId}";
        public static string ForgotSession(string sessionToken) => $"{ForgotSessionPrefix}{sessionToken}";
        public static string ResetToken(string resetToken) => $"{ResetTokenPrefix}{resetToken}";
    }
}
