using System.Net;

namespace Quizer.Application.Common.Exceptions
{
    public class ConflictException : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public string ErrorMessage { get; private set; }

        public ConflictException(string message) : base(message)
        {
            ErrorMessage = message;
        }
    }
}
