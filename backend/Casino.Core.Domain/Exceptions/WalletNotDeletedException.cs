using SharedKernel.Exceptions;
using System.Net;

namespace Casino.Core.Domain.Exceptions
{
    public class WalletNotDeletedException: BaseException
    {
        private const string DefaultErrorCode = "WALLET_NOT_DELETED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public WalletNotDeletedException(string message = "Wallet is not deleted.")
            : base(message, DefaultErrorCode) { }
        public WalletNotDeletedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public WalletNotDeletedException(Exception innerException)
            : base("Wallet is not deleted.", innerException, DefaultErrorCode) { }
    }
}
