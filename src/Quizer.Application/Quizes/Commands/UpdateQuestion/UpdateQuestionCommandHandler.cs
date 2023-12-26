using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Application.Quizes.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, ErrorOr<QuestionId>>
{
    private readonly IQuizRepository _quizRepository;

    public UpdateQuestionCommandHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<QuestionId>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _quizRepository.GetQuestion(QuestionId.Create(request.QuestionId));
        if (question is null) return Errors.Question.NotFound;

        question.Update(request.QuestionText, request.Answer);

        return question.Id;
    }
}
