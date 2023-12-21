using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Quizes.Commands.CreateQuiz;
using Quizer.Application.Quizes.Queries.GetQuizes;
using Quizer.Contracts.Quiz;

namespace Quizer.Api.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class QuizController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public QuizController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuizes(string? userId = null)
        {
            var query = new GetQuizesQuery(userId);
            var result = await _mediator.Send(query);
            
            return result.Match(
                quizes => Ok(_mapper.Map<List<QuizResponse>>(quizes)),
                Problem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz(
            CreateQuizRequest request)
        {
            var command = _mapper.Map<CreateQuizCommand>(request);
            var result = await _mediator.Send(command);

            return result.Match(
                quiz => Ok(_mapper.Map<QuizResponse>(quiz)),
                Problem);
        }
    }
}
