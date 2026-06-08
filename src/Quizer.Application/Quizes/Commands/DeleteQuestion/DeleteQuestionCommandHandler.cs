using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandler : ICommandHandler<DeleteQuestionCommand, ErrorOr<QuestionId>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuizRepository _quizRepository;

    public DeleteQuestionCommandHandler(IQuestionRepository questionRepository, IQuizRepository quizRepository)
    {
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<QuestionId>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.Get(QuestionId.Create(request.QuestionId));
        if (question is null) return Errors.Question.NotFound;

        var quiz = await _quizRepository.Get(question.QuizId);
        if (quiz is null) return Errors.Quiz.NotFound;

        if (quiz.CreatedBy != request.UserId)
            return Errors.Quiz.Forbidden;

        quiz.DeleteQuestion(request.UserId, question);
        _questionRepository.Delete(question);
        question.Delete();

        return (QuestionId)question.Id;
    }
}
