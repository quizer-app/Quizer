using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Application.Quizes.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, ErrorOr<QuestionId>>
{
    private readonly IQuizRepository _quizRepository;

    public DeleteQuestionCommandHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<QuestionId>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _quizRepository.GetQuestion(QuestionId.Create(request.QuestionId));
        if (question is null) return Errors.Question.NotFound;

        _quizRepository.DeleteQuestion(question);

        return question.Id;
    }
}
