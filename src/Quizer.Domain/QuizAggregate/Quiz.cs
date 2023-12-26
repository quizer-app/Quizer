using ErrorOr;
using Quizer.Domain.Common.Models;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuizAggregate.Events;
using Quizer.Domain.QuizAggregate.Validation;

namespace Quizer.Domain.QuizAggregate;

public sealed class Quiz : AggregateRoot<QuizId, Guid>
{
    private readonly List<QuestionId> _questionIds = new();

    public Guid UserId { get; private set; }
    public string UserName { get; private set; }

    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string Description { get; private set; }
    public AverageRating AverageRating { get; private set; }
    public IReadOnlyList<QuestionId> QuestionIds => _questionIds.AsReadOnly();

    private Quiz(
        QuizId id,
        string name,
        string slug,
        string description,
        Guid userId,
        string userName,
        AverageRating averageRating) : base(id)
    {
        Name = name;
        Slug = slug;
        Description = description;
        UserId = userId;
        UserName = userName;
        AverageRating = averageRating;
    }

    private ErrorOr<bool> Validate()
    {
        var validator = new QuizValidator();
        var validationResult = validator.Validate(this);
        return base.GetValidationErrors(validationResult);
    }

    public static ErrorOr<Quiz> Create(
        string name,
        string slug,
        string description,
        Guid userId,
        string userName)
    {
        var quiz = new Quiz(
            QuizId.CreateUnique(),
            name,
            slug,
            description,
            userId,
            userName,
            AverageRating.CreateNew());

        var result = quiz.Validate();
        if (result.IsError) return result.Errors;

        return quiz;
    }

    public ErrorOr<bool> Update(
        string name,
        string slug,
        string description)
    {
        base.Update();
        Name = name;
        Description = description;
        Slug = slug;

        var result = this.Validate();
        if (result.IsError) return result.Errors;

        return true;
    }

    public void Delete()
    {
        this.AddDomainEvent(new QuizDeleted(this));
    }

    public void AddQuestion(QuestionId questionId)
    {
        _questionIds.Add(questionId);
    }

    public void DeleteQuestion(QuestionId questionId)
    {
        _questionIds.Remove(questionId);
    }

#pragma warning disable CS8618
    private Quiz()
    {
    }
#pragma warning restore CS8618
}
