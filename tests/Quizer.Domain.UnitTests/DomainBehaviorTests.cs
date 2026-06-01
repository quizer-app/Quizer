using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuestionAggregate.Entities;
using Quizer.Domain.QuizAggregate;
using Xunit;

namespace Quizer.Domain.UnitTests;

public class DomainBehaviorTests
{
    [Fact]
    public void Quiz_create_with_valid_data_sets_core_fields()
    {
        var userId = Guid.NewGuid();
        var image = Image.CreateNew(Guid.NewGuid(), new Uri("https://cdn.example.com/q.png"));

        var result = Quiz.Create(userId, image, "Networking Basics", "networking-basics", "A quiz", "alice", 900, true, 3);

        Assert.False(result.IsError);
        var quiz = result.Value;
        Assert.Equal("alice/networking-basics", quiz.Location);
        Assert.Equal(0, quiz.NumberOfQuestions);
        Assert.Equal(userId, quiz.CreatedBy);
        Assert.Equal(900, quiz.TimeLimitInSeconds);
        Assert.True(quiz.AutoSubmitWhenTimeExpires);
        Assert.Equal(3, quiz.MaxAttemptsPerUser);
    }

    [Fact]
    public void Quiz_create_with_empty_name_returns_validation_error()
    {
        var image = Image.CreateNew(Guid.NewGuid(), new Uri("https://cdn.example.com/q.png"));

        var result = Quiz.Create(Guid.NewGuid(), image, string.Empty, "slug", "A quiz", "alice", null, false, null);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == "Name");
    }

    [Fact]
    public void Quiz_create_with_auto_submit_and_no_limit_returns_validation_error()
    {
        var image = Image.CreateNew(Guid.NewGuid(), new Uri("https://cdn.example.com/q.png"));

        var result = Quiz.Create(Guid.NewGuid(), image, "Name", "slug", "A quiz", "alice", null, true, null);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, error => error.Code == "AutoSubmitWhenTimeExpires");
    }

    [Fact]
    public void Quiz_attempt_limit_is_enforced()
    {
        var image = Image.CreateNew(Guid.NewGuid(), new Uri("https://cdn.example.com/q.png"));
        var quiz = Quiz.Create(Guid.NewGuid(), image, "Name", "slug", "A quiz", "alice", 600, true, 2).Value;

        Assert.False(quiz.CanStartAttempt(1).IsError);
        Assert.True(quiz.CanStartAttempt(2).IsError);
    }

    [Fact]
    public void Quiz_is_attempt_expired_when_time_passes_deadline()
    {
        var image = Image.CreateNew(Guid.NewGuid(), new Uri("https://cdn.example.com/q.png"));
        var quiz = Quiz.Create(Guid.NewGuid(), image, "Name", "slug", "A quiz", "alice", 300, true, null).Value;
        var startedAt = DateTimeOffset.UtcNow;

        Assert.False(quiz.IsAttemptExpired(startedAt, startedAt.AddSeconds(299)));
        Assert.True(quiz.IsAttemptExpired(startedAt, startedAt.AddSeconds(300)));
    }

    [Fact]
    public void Question_create_adds_created_domain_event()
    {
        var answers = new List<Answer> { Answer.Create("TCP", true).Value };

        var result = Question.Create(Guid.NewGuid(), QuizId.CreateUnique(), "What protocol is reliable?", answers);

        Assert.False(result.IsError);
        Assert.Single(result.Value.DomainEvents);
    }

    [Fact]
    public void Question_create_without_correct_answer_returns_validation_error()
    {
        var answers = new List<Answer>
        {
            Answer.Create("A", false).Value,
            Answer.Create("B", false).Value
        };

        var result = Question.Create(Guid.NewGuid(), QuizId.CreateUnique(), "Which options are valid?", answers);

        Assert.True(result.IsError);
    }

    [Fact]
    public void Average_rating_add_and_remove_keeps_consistent_value()
    {
        var average = AverageRating.CreateNew();
        var first = Rating.Create(4);
        var second = Rating.Create(2);

        average.AddNewRating(first);
        average.AddNewRating(second);
        average.RemoveRating(second);

        Assert.Equal(4, average.Value);
        Assert.Equal(1, average.NumRatings);
    }
}
