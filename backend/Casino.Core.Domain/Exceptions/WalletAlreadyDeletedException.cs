using SharedKernel.Exceptions;
using System.Net;

namespace Casino.Core.Domain.Exceptions
{
    public class WalletAlreadyDeletedException : BaseException
    {
        private const string DefaultErrorCode = "WALLET_ALREADY_DELETED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public WalletAlreadyDeletedException(string message = "Wallet already deleted.")
            : base(message, DefaultErrorCode) { }
        public WalletAlreadyDeletedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public WalletAlreadyDeletedException(Exception innerException)
            : base("Wallet already deleted.", innerException, DefaultErrorCode) { }
    }

}

