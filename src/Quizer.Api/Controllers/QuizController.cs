using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quizer.Api.Controllers
{
    [Route("[controller]")]
    public class QuizController : ApiController
    {
        [HttpGet]
        public IActionResult ListQuizes()
        {
            return Ok(Array.Empty<string>());
        }
    }
}
