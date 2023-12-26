using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Quizes.Commands.CreateQuestion;
using Quizer.Application.Quizes.Commands.DeleteQuestion;
using Quizer.Application.Quizes.Commands.UpdateQuestion;
using Quizer.Application.Quizes.Queries.GetQuestion;
using Quizer.Contracts.Quiz;

namespace Quizer.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]

public class QuestionController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public QuestionController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetQuestion(Guid id)
    {
        var query = new GetQuestionQuery(id);
        var result = await _mediator.Send(query);

        return result.Match(
            quizes => Ok(_mapper.Map<QuestionResponse>(quizes)),
            Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuestion(
        CreateQuestionRequest request)
    {
        var command = _mapper.Map<CreateQuestionCommand>(request);
        var result = await _mediator.Send(command);

        return result.Match(
            quizId => Ok(_mapper.Map<QuestionIdResponse>(quizId)),
            Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateQuestion(
        Guid id,
        [FromBody] UpdateQuestionRequest request)
    {
        var command = _mapper.Map<UpdateQuestionCommand>((request, id));
        var result = await _mediator.Send(command);

        return result.Match(
            quizId => Ok(_mapper.Map<QuestionIdResponse>(quizId)),
            Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteQuestion(Guid id)
    {
        var command = new DeleteQuestionCommand(id);
        var result = await _mediator.Send(command);

        return result.Match(
            quizId => Ok(_mapper.Map<QuestionIdResponse>(quizId)),
            Problem);
    }
}
