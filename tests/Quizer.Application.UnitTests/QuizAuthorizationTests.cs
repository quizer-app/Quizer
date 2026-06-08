using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Quizer.Application.Quizes.Commands.CreateQuiz;
using ErrorOr;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Quizes.Commands.CreateQuestion;
using Quizer.Application.Quizes.Commands.DeleteQuestion;
using Quizer.Application.Quizes.Commands.DeleteQuiz;
using Quizer.Application.Quizes.Commands.UpdateQuestion;
using Quizer.Application.Quizes.Commands.UpdateQuiz;
using Quizer.Application.Quizes.Common;
using Quizer.Application.Services.Image;
using Quizer.Application.Services.Image.Response;
using Quizer.Application.Services.Slugify;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuestionAggregate.Entities;
using Quizer.Domain.QuizAggregate;
using Xunit;

namespace Quizer.Application.UnitTests;

public class QuizAuthorizationTests
{
    [Fact]
    public async Task Create_quiz_rejects_duplicate_generated_slug()
    {
        var userId = Guid.NewGuid();
        var existingQuiz = Quiz.Create(
            userId,
            Image.CreateNew(Guid.NewGuid(), new Uri("https://cdn.example.com/existing.png")),
            "My Quiz",
            "my-quiz",
            "Description",
            "alice",
            null,
            false,
            null).Value;
        var handler = new CreateQuizCommandHandler(
            new InMemoryQuizRepository(existingQuiz),
            new HyphenSlugifyService(),
            CreateUserManager(new Quizer.Domain.UserAggregate.User { Id = userId.ToString(), UserName = "alice" }),
            new SuccessfulImageService());

        var result = await handler.Handle(
            new CreateQuizCommand("my quiz", "Description", userId, Guid.NewGuid(), null, false, null),
            CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == "Quiz.DuplicateName");
    }

    [Fact]
    public async Task Update_quiz_rejects_non_owner()
    {
        var ownerId = Guid.NewGuid();
        var attackerId = Guid.NewGuid();
        var quiz = CreateQuiz(ownerId);
        var handler = new UpdateQuizCommandHandler(
            new InMemoryQuizRepository(quiz),
            new ConstantSlugifyService(),
            new SuccessfulImageService());

        var result = await handler.Handle(
            new UpdateQuizCommand(attackerId, quiz.Image.ImageId, ((QuizId)quiz.Id).Value, "Changed", "Changed", null, false, null),
            CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == "Quiz.Forbidden");
        Assert.Equal("Original", quiz.Name);
    }

    [Fact]
    public async Task Delete_quiz_rejects_non_owner_without_deleting_image_or_quiz()
    {
        var ownerId = Guid.NewGuid();
        var attackerId = Guid.NewGuid();
        var quiz = CreateQuiz(ownerId);
        var quizRepository = new InMemoryQuizRepository(quiz);
        var imageService = new SuccessfulImageService();
        var handler = new DeleteQuizCommandHandler(quizRepository, imageService);

        var result = await handler.Handle(
            new DeleteQuizCommand(attackerId, ((QuizId)quiz.Id).Value),
            CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == "Quiz.Forbidden");
        Assert.False(quizRepository.WasDeleted);
        Assert.False(imageService.DeleteWasCalled);
    }

    [Fact]
    public async Task Create_question_rejects_non_owner()
    {
        var ownerId = Guid.NewGuid();
        var attackerId = Guid.NewGuid();
        var quiz = CreateQuiz(ownerId);
        var questionRepository = new InMemoryQuestionRepository();
        var handler = new CreateQuestionCommandHandler(new InMemoryQuizRepository(quiz), questionRepository);

        var result = await handler.Handle(
            new CreateQuestionCommand(
                attackerId,
                ((QuizId)quiz.Id).Value,
                "Question?",
                new List<CreateQuestionAnswer> { new("Correct", true) }),
            CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == "Quiz.Forbidden");
        Assert.Null(questionRepository.Question);
        Assert.Empty(quiz.QuestionIds);
    }

    [Fact]
    public async Task Update_question_rejects_non_owner()
    {
        var ownerId = Guid.NewGuid();
        var attackerId = Guid.NewGuid();
        var quiz = CreateQuiz(ownerId);
        var question = CreateQuestion(ownerId, (QuizId)quiz.Id);
        var handler = new UpdateQuestionCommandHandler(
            new InMemoryQuestionRepository(question),
            new InMemoryQuizRepository(quiz));

        var result = await handler.Handle(
            new UpdateQuestionCommand(
                attackerId,
                ((QuestionId)question.Id).Value,
                "Changed?",
                new List<CreateQuestionAnswer> { new("Changed", true) }),
            CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == "Quiz.Forbidden");
        Assert.Equal("Original question?", question.QuestionText);
    }

    [Fact]
    public async Task Delete_question_rejects_non_owner()
    {
        var ownerId = Guid.NewGuid();
        var attackerId = Guid.NewGuid();
        var quiz = CreateQuiz(ownerId);
        var question = CreateQuestion(ownerId, (QuizId)quiz.Id);
        quiz.AddQuestion(ownerId, question);
        var questionRepository = new InMemoryQuestionRepository(question);
        var handler = new DeleteQuestionCommandHandler(questionRepository, new InMemoryQuizRepository(quiz));

        var result = await handler.Handle(
            new DeleteQuestionCommand(attackerId, ((QuestionId)question.Id).Value),
            CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == "Quiz.Forbidden");
        Assert.False(questionRepository.WasDeleted);
        Assert.Single(quiz.QuestionIds);
    }

    private static Quiz CreateQuiz(Guid ownerId)
    {
        return Quiz.Create(
            ownerId,
            Image.CreateNew(Guid.NewGuid(), new Uri("https://cdn.example.com/quiz.png")),
            "Original",
            "original",
            "Description",
            "alice",
            null,
            false,
            null).Value;
    }

    private static Question CreateQuestion(Guid ownerId, QuizId quizId)
    {
        return Question.Create(
            ownerId,
            quizId,
            "Original question?",
            new List<Answer> { Answer.Create("Correct", true).Value }).Value;
    }

    private sealed class InMemoryQuizRepository : IQuizRepository
    {
        private readonly Quiz? _quiz;

        public InMemoryQuizRepository(Quiz? quiz)
        {
            _quiz = quiz;
        }

        public IQueryable<Quiz> Quizes => _quiz is null
            ? Enumerable.Empty<Quiz>().AsQueryable()
            : new[] { _quiz }.AsQueryable();

        public bool WasDeleted { get; private set; }

        public Task Add(Quiz quiz) => Task.CompletedTask;

        public void Delete(Quiz quiz)
        {
            WasDeleted = true;
        }

        public Task<Quiz?> Get(QuizId id)
        {
            return Task.FromResult(_quiz is not null && Equals(_quiz.Id, id) ? _quiz : null);
        }

        public Task<Quiz?> Get(string userName, string quizSlug)
        {
            return Task.FromResult(_quiz is not null && _quiz.UserName == userName && _quiz.Slug == quizSlug ? _quiz : null);
        }
    }

    private sealed class InMemoryQuestionRepository : IQuestionRepository
    {
        public InMemoryQuestionRepository(Question? question = null)
        {
            Question = question;
        }

        public Question? Question { get; private set; }
        public bool WasDeleted { get; private set; }

        public Task Add(Question question)
        {
            Question = question;
            return Task.CompletedTask;
        }

        public void Delete(Question question)
        {
            WasDeleted = true;
        }

        public Task<Question?> Get(QuestionId questionId)
        {
            return Task.FromResult(Question is not null && Equals(Question.Id, questionId) ? Question : null);
        }

        public Task<List<Question>> GetAllQuestions(QuizId quizId)
        {
            var questions = Question is not null && Equals(Question.QuizId, quizId)
                ? new List<Question> { Question }
                : new List<Question>();

            return Task.FromResult(questions);
        }
    }

    private sealed class SuccessfulImageService : IImageService
    {
        public bool DeleteWasCalled { get; private set; }

        public Task<ErrorOr<DirectUploadResponse>> DirectUpload(bool requireSignedURLs = false)
        {
            throw new NotSupportedException();
        }

        public Task<bool> IsSuccessfulyUploaded(Guid imageId) => Task.FromResult(true);

        public Task<bool> DeleteImage(Guid imageId)
        {
            DeleteWasCalled = true;
            return Task.FromResult(true);
        }

        public Uri GenerateImageUrl(Guid imageId, string variantName)
        {
            return new Uri($"https://cdn.example.com/{variantName}/{imageId}");
        }
    }

    private sealed class ConstantSlugifyService : ISlugifyService
    {
        public string GenerateSlug(string phrase) => phrase.ToLowerInvariant();
    }

    private static UserManager<Quizer.Domain.UserAggregate.User> CreateUserManager(Quizer.Domain.UserAggregate.User user)
    {
        return new UserManager<Quizer.Domain.UserAggregate.User>(
            new SingleUserStore(user),
            Options.Create(new IdentityOptions()),
            new PasswordHasher<Quizer.Domain.UserAggregate.User>(),
            Array.Empty<IUserValidator<Quizer.Domain.UserAggregate.User>>(),
            Array.Empty<IPasswordValidator<Quizer.Domain.UserAggregate.User>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            EmptyServiceProvider.Instance,
            NullLogger<UserManager<Quizer.Domain.UserAggregate.User>>.Instance);
    }
    private sealed class EmptyServiceProvider : IServiceProvider
    {
        public static readonly EmptyServiceProvider Instance = new();

        public object? GetService(Type serviceType) => null;
    }

    private sealed class SingleUserStore : IUserStore<Quizer.Domain.UserAggregate.User>
    {
        private readonly Quizer.Domain.UserAggregate.User _user;

        public SingleUserStore(Quizer.Domain.UserAggregate.User user)
        {
            _user = user;
        }

        public void Dispose()
        {
        }

        public Task<string> GetUserIdAsync(Quizer.Domain.UserAggregate.User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string?> GetUserNameAsync(Quizer.Domain.UserAggregate.User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(Quizer.Domain.UserAggregate.User user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(Quizer.Domain.UserAggregate.User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(Quizer.Domain.UserAggregate.User user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(Quizer.Domain.UserAggregate.User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(Quizer.Domain.UserAggregate.User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(Quizer.Domain.UserAggregate.User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<Quizer.Domain.UserAggregate.User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_user.Id == userId ? _user : null);
        }

        public Task<Quizer.Domain.UserAggregate.User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_user.NormalizedUserName == normalizedUserName ? _user : null);
        }
    }

    private sealed class HyphenSlugifyService : ISlugifyService
    {
        public string GenerateSlug(string phrase) => phrase.Replace(' ', '-').ToLowerInvariant();
    }

}
