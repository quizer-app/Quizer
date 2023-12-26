using ErrorOr;
using FluentValidation.Results;
using Quizer.Domain.Common.Models;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuizAggregate.Entities;
using Quizer.Domain.QuizAggregate.Events;
using Quizer.Domain.QuizAggregate.Validation;

namespace Quizer.Domain.QuizAggregate;

public sealed class Quiz : AggregateRoot<QuizId, Guid>
{
    private readonly List<Question> _questions = new();

    public Guid UserId { get; private set; }
    public string UserName { get; private set; }

    public string Name { get; private set; }
    public string Slug { get; private set; }
    public string Description { get; private set; }
    public AverageRating AverageRating { get; private set; }
    public IReadOnlyList<Question> Questions => _questions.AsReadOnly();

    private Quiz(
        QuizId id,
        string name,
        string slug,
        string description,
        Guid userId,
        string userName,
        AverageRating averageRating,
        List<Question> questions) : base(id)
    {
        Name = name;
        Slug = slug;
        Description = description;
        UserId = userId;
        UserName = userName;
        AverageRating = averageRating;
        _questions = questions;
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
        string userName,
        AverageRating averageRating,
        List<Question> questions)
    {
        var quiz = new Quiz(
            QuizId.CreateUnique(),
            name,
            slug,
            description,
            userId,
            userName,
            averageRating,
            questions);

        var result = quiz.Validate();
        if (result.IsError) return result.Errors;

        quiz.AddDomainEvent(new QuizCreated(quiz));

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

        this.AddDomainEvent(new QuizUpdated(this));

        return true;
    }

    public void Delete()
    {
        this.AddDomainEvent(new QuizDeleted(this));
    }

    public void AddQuestion(Question question)
    {
        _questions.Add(question);
    }

#pragma warning disable CS8618
    private Quiz()
    {
    }
#pragma warning restore CS8618
}
