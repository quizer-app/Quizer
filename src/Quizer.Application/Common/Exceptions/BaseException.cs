using System.Net;

namespace Quizer.Application.Common.Exceptions
{
    public abstract class BaseException : Exception, IServiceException
    {
        public virtual HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
        public string ErrorMessage { get; private set; }

        public BaseException(string message) : base(message)
        {
            ErrorMessage = message;
        }
    }
}
