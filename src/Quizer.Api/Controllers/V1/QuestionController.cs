using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Quizes.Commands.CreateQuestion;
using Quizer.Application.Quizes.Commands.DeleteQuestion;
using Quizer.Application.Quizes.Commands.UpdateQuestion;
using Quizer.Application.Quizes.Queries.GetQuestion;
using Quizer.Contracts.Question;

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

    [HttpGet("{questionId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetQuestion(Guid questionId)
    {
        var query = new GetQuestionQuery(questionId);
        var result = await _mediator.Send(query);

        return result.Match(
            quizes => Ok(_mapper.Map<QuestionResponse>(quizes)),
            Problem);
    }

    [HttpPost("{quizId:guid}")]
    public async Task<IActionResult> CreateQuestion(
        Guid quizId,
        [FromBody] CreateQuestionRequest request)
    {
        var command = _mapper.Map<CreateQuestionCommand>((request, UserId, quizId));
        var result = await _mediator.Send(command);

        return result.Match(
            quizId => Ok(_mapper.Map<QuestionIdResponse>(quizId)),
            Problem);
    }

    [HttpPut("{questionId:guid}")]
    public async Task<IActionResult> UpdateQuestion(
        Guid questionId,
        [FromBody] UpdateQuestionRequest request)
    {
        var command = _mapper.Map<UpdateQuestionCommand>((request, UserId, questionId));
        var result = await _mediator.Send(command);

        return result.Match(
            quizId => Ok(_mapper.Map<QuestionIdResponse>(quizId)),
            Problem);
    }

    [HttpDelete("{questionId:guid}")]
    public async Task<IActionResult> DeleteQuestion(Guid questionId)
    {
        var command = new DeleteQuestionCommand((Guid)UserId!, questionId);
        var result = await _mediator.Send(command);

        return result.Match(
            quizId => Ok(_mapper.Map<QuestionIdResponse>(quizId)),
            Problem);
    }
}
