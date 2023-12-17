using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Quizer.Api.Controllers
{
    public class ErrorsController : ControllerBase
    {
        [Route("errors")]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()!.Error;
            return Problem();
        }
    }
}
