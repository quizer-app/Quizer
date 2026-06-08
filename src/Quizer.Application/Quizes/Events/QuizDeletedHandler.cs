using Quizer.Application.Common.Cqrs;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Events;

namespace Quizer.Application.Quizes.Events;

public class QuizDeletedHandler : IDomainEventHandler<QuizDeleted>
{
    private readonly IQuestionRepository _questionRepository;

    public QuizDeletedHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task Handle(QuizDeleted domainEvent, CancellationToken cancellationToken)
    {
        var quizId = (QuizId)domainEvent.quiz.Id;
        var questions = await _questionRepository.GetAllQuestions(quizId);

        foreach (var question in questions)
        {
            question.Delete();
            _questionRepository.Delete(question);
        }
    }
}
