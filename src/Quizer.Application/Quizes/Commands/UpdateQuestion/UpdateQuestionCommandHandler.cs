using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuestionAggregate.Entities;

namespace Quizer.Application.Quizes.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, ErrorOr<QuestionId>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuizRepository _quizRepository;

    public UpdateQuestionCommandHandler(IQuestionRepository questionRepository, IQuizRepository quizRepository)
    {
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<QuestionId>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.Get(QuestionId.Create(request.QuestionId));
        if (question is null) return Errors.Question.NotFound;

        var quiz = await _quizRepository.Get(question.QuizId);
        if (quiz is null) return Errors.Quiz.NotFound;
        if (quiz.CreatedBy != request.UserId) return Errors.Quiz.Forbidden;

        var result = question.Update(
            request.UserId,
            request.QuestionText,
            request.Answers
                .ConvertAll(a => Answer.Create(a.Text, a.IsCorrect).Value));

        if (result.IsError) return result.Errors;

        return (QuestionId)question.Id;
    }
}
