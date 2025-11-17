using SharedKernel.Exceptions;
using System.Net;


namespace Casino.Core.Domain.Exceptions
{
    public class WalletAlreadyBannedException : BaseException
    {
        private const string DefaultErrorCode = "WALLET_ALREADY_BANNED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public WalletAlreadyBannedException(string message = "Wallet already banned.")
            : base(message, DefaultErrorCode) { }
        public WalletAlreadyBannedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public WalletAlreadyBannedException(Exception innerException)
            : base("Wallet already banned.", innerException, DefaultErrorCode) { }
    }
}
