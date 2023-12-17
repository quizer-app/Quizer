using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Common.Exceptions;

namespace Quizer.Api.Controllers
{
    [ApiController]
    [Route("error")]
    public class ErrorsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()!.Error;

            var (statusCode, message) = exception switch
            {
                IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
                _ => (StatusCodes.Status500InternalServerError, "Internal server error")
            };

            return Problem(
                statusCode: statusCode,
                title: message);
        }
    }
}
