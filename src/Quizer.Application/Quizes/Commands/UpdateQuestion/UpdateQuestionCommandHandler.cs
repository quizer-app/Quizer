using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, ErrorOr<QuestionId>>
{
    private readonly IQuestionRepository _questionRepository;

    public UpdateQuestionCommandHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<ErrorOr<QuestionId>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.Get(QuestionId.Create(request.QuestionId));
        if (question is null) return Errors.Question.NotFound;

        // TODO: update
        //question.Update(request.QuestionText, request.Answer);

        //return question.Id;

        return QuestionId.CreateUnique();
    }
}
