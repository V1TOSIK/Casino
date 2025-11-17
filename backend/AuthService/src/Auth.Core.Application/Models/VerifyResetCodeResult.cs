namespace Auth.Core.Application.Models
{
    public class VerifyResetCodeResult
    {
        public string ResetToken { get; set; } = string.Empty;

        public VerifyResetCodeResult(string resetToken)
        {
            ResetToken = resetToken;
        }
    }
}
