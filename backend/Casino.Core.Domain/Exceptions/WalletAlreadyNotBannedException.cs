using SharedKernel.Exceptions;
using System.Net;

namespace Casino.Core.Domain.Exceptions
{
    public class WalletAlreadyNotBannedException: BaseException
    {
        private const string DefaultErrorCode = "WALLET_ALREADY_NOT_BANNED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public WalletAlreadyNotBannedException(string message = "Wallet is already not banned.")
            : base(message, DefaultErrorCode) { }
        public WalletAlreadyNotBannedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public WalletAlreadyNotBannedException(Exception innerException)
            : base("Wallet is already not banned.", innerException, DefaultErrorCode) { }
    }
}
