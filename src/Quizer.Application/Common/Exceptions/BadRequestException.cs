using System.Net;

namespace Quizer.Application.Common.Exceptions
{
    public class BadRequestException : BaseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public BadRequestException(string message) : base(message) { }
    }
}
