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

    public string UserName { get; private set; }

    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string Description { get; private set; }
    public AverageRating AverageRating { get; private set; }
    
    public string Location => $"{UserName}/{Slug}";
    public IReadOnlyList<QuestionId> QuestionIds => _questionIds.AsReadOnly();

    private Quiz(
        Guid userId,
        QuizId id,
        string name,
        string slug,
        string description,
        string userName,
        AverageRating averageRating) : base(id, userId)
    {
        Name = name;
        Slug = slug;
        Description = description;
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
        Guid userId,
        string name,
        string slug,
        string description,
        string userName)
    {
        var quiz = new Quiz(
            userId,
            QuizId.CreateUnique(),
            name,
            slug,
            description,
            userName,
            AverageRating.CreateNew());

        var result = quiz.Validate();
        if (result.IsError) return result.Errors;

        return quiz;
    }

    public ErrorOr<bool> Update(
        Guid userId,
        string name,
        string slug,
        string description)
    {
        base.Update(userId);
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

    public void AddQuestion(Guid userId, Question question)
    {
        base.Update(userId);
        _questionIds.Add((QuestionId)question.Id);
    }

    public void DeleteQuestion(Guid userId, Question question)
    {
        base.Update(userId);
        _questionIds.Remove((QuestionId)question.Id);
    }

#pragma warning disable CS8618
    private Quiz() {}
#pragma warning restore CS8618
}
