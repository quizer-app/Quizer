using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Quizer.Api.Controllers;

[ApiController]
[Route("error")]
public class ErrorsController : ControllerBase
{
    [HttpPost]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        if (feature == null)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }

        Exception? exception = feature.Error;

        var (statusCode, message) = exception switch
        {
            _ => (StatusCodes.Status500InternalServerError, "Internal server error")
        };

        return Problem(
            statusCode: statusCode,
            title: message);
    }
}
