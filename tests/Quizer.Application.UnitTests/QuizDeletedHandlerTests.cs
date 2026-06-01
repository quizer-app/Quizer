using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Quizes.Events;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuestionAggregate.Entities;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Events;
using Xunit;

namespace Quizer.Application.UnitTests;

public class QuizDeletedHandlerTests
{
    [Fact]
    public async Task Handle_deletes_all_questions_for_deleted_quiz()
    {
        var quiz = CreateQuiz();
        var first = CreateQuestion((QuizId)quiz.Id, "First");
        var second = CreateQuestion((QuizId)quiz.Id, "Second");
        var repository = new FakeQuestionRepository(new List<Question> { first, second });
        var handler = new QuizDeletedHandler(repository);

        await handler.Handle(new QuizDeleted(quiz), CancellationToken.None);

        Assert.Equal(2, repository.Deleted.Count);
        var deletedIds = repository.Deleted.Select(question => question.Id).ToHashSet();
        Assert.Contains(first.Id, deletedIds);
        Assert.Contains(second.Id, deletedIds);
        Assert.All(repository.Deleted, question => Assert.Contains(question.DomainEvents, domainEvent => domainEvent is Quizer.Domain.QuestionAggregate.Events.QuestionDeleted));
    }

    private static Quiz CreateQuiz()
    {
        var quiz = Quiz.Create(Guid.NewGuid(), Image.CreateNew(Guid.NewGuid(), new Uri("https://example.com/image.png")), "Quiz", "quiz", "Description", "owner", null, false, null);

        return quiz.Value;
    }

    private static Question CreateQuestion(QuizId quizId, string text)
    {
        var answers = new List<Answer>
        {
            Answer.Create("A", true).Value,
            Answer.Create("B").Value
        };

        var question = Question.Create(Guid.NewGuid(), quizId, text, answers);
        return question.Value;
    }

    private sealed class FakeQuestionRepository : IQuestionRepository
    {
        private readonly List<Question> _questions;

        public List<Question> Deleted { get; } = new();

        public FakeQuestionRepository(List<Question> questions)
        {
            _questions = questions;
        }

        public Task Add(Question question)
        {
            _questions.Add(question);
            return Task.CompletedTask;
        }

        public void Delete(Question question)
        {
            Deleted.Add(question);
        }

        public Task<Question?> Get(QuestionId questionId)
        {
            return Task.FromResult(_questions.FirstOrDefault(q => q.Id == questionId));
        }

        public Task<List<Question>> GetAllQuestions(QuizId quizId)
        {
            var questions = _questions.Where(q => q.QuizId == quizId).ToList();
            return Task.FromResult(questions);
        }
    }
}
