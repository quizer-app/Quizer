using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Events;

namespace Quizer.Application.Quizes.Events;

public class QuizDeletedHandler : INotificationHandler<QuizDeleted>
{
    private readonly IQuestionRepository _questionRepository;

    public QuizDeletedHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task Handle(QuizDeleted notification, CancellationToken cancellationToken)
    {
        var quizId = (QuizId)notification.quiz.Id;
        var questions = await _questionRepository.GetAllQuestions(quizId);

        foreach (var question in questions)
        {
            question.Delete();
            _questionRepository.Delete(question);
        }
    }
}
