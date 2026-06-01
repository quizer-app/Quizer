using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuestionAggregate.Entities;
using Quizer.Domain.QuizAggregate;
using Quizer.Infrastructure.Persistance;
using Quizer.Infrastructure.Persistance.Interceptors;
using Quizer.Infrastructure.Persistance.Repositories;
using Xunit;

namespace Quizer.Infrastructure.IntegrationTests;

public class PersistenceIntegrationTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<QuizerDbContext> _options;

    public PersistenceIntegrationTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<QuizerDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    [Fact]
    public async Task Quiz_repository_persists_and_loads_quiz_by_id_and_slug()
    {
        using var context = CreateContext();
        var repository = new QuizRepository(context);
        var unitOfWork = new UnitOfWork(context);

        var quiz = Quiz.Create(Guid.NewGuid(), Image.CreateNew(Guid.NewGuid(), new Uri("https://cdn.example.com/quiz.png")), "CSharp Basics", "csharp-basics", "Language fundamentals", "alice", null, false, null).Value;

        await repository.Add(quiz);
        await unitOfWork.SaveChangesAsync();

        var byId = await repository.Get((QuizId)quiz.Id);
        var bySlug = await repository.Get("alice", "csharp-basics");

        Assert.NotNull(byId);
        Assert.NotNull(bySlug);
        Assert.Equal(((QuizId)quiz.Id).Value, ((QuizId)byId!.Id).Value);
    }

    [Fact]
    public async Task Question_repository_returns_only_questions_for_requested_quiz()
    {
        using var context = CreateContext();
        var quizRepository = new QuizRepository(context);
        var questionRepository = new QuestionRepository(context);
        var unitOfWork = new UnitOfWork(context);

        var quizOne = Quiz.Create(Guid.NewGuid(), Image.CreateNew(Guid.NewGuid(), new Uri("https://cdn.example.com/1.png")), "Quiz One", "quiz-one", "Desc", "alice", null, false, null).Value;
        var quizTwo = Quiz.Create(Guid.NewGuid(), Image.CreateNew(Guid.NewGuid(), new Uri("https://cdn.example.com/2.png")), "Quiz Two", "quiz-two", "Desc", "bob", null, false, null).Value;

        await quizRepository.Add(quizOne);
        await quizRepository.Add(quizTwo);

        var answer = Answer.Create("42", true).Value;
        var questionOne = Question.Create(Guid.NewGuid(), (QuizId)quizOne.Id, "Q1", new List<Answer> { answer }).Value;
        var questionTwo = Question.Create(Guid.NewGuid(), (QuizId)quizTwo.Id, "Q2", new List<Answer> { answer }).Value;

        await questionRepository.Add(questionOne);
        await questionRepository.Add(questionTwo);
        await unitOfWork.SaveChangesAsync();

        var quizOneQuestions = await questionRepository.GetAllQuestions((QuizId)quizOne.Id);

        Assert.Single(quizOneQuestions);
        Assert.Equal(((QuestionId)questionOne.Id).Value, ((QuestionId)quizOneQuestions[0].Id).Value);
    }
    private QuizerDbContext CreateContext()
    {
        return new QuizerDbContext(_options, new PublishDomainEventsInterceptor(new NullPublisher()));
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    private sealed class NullPublisher : IPublisher
    {
        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            return Task.CompletedTask;
        }
    }
}
