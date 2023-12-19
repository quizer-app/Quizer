using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Quizer.Api.Common.Http;

namespace Quizer.Api.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            HttpContext.Items.Add(HttpContextItemKeys.Errors, errors);
            var firstError = errors[0];

            var statusCode = firstError.Type switch
            {
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, title: firstError.Description);
        }
    }
}
