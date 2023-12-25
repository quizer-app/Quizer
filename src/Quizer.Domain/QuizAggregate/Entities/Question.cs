using ErrorOr;
using Quizer.Domain.Common.Models;
using Quizer.Domain.QuizAggregate.Events;
using Quizer.Domain.QuizAggregate.Validation;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Domain.QuizAggregate.Entities;

public sealed class Question : Entity<QuestionId>
{
    public string QuestionText { get; private set; }
    public string Answer { get; private set; }

    private Question(QuestionId id, string questionText, string answer) : base(id)
    {
        QuestionText = questionText;
        Answer = answer;
    }

    private ErrorOr<bool> Validate()
    {
        var validator = new QuestionValidator();
        var validationResult = validator.Validate(this);
        return base.GetValidationErrors(validationResult);
    }

    public static ErrorOr<Question> Create(string questionText, string answer)
    {
        var question = new Question(QuestionId.CreateUnique(), questionText, answer);

        var result = question.Validate();
        if (result.IsError) return result.Errors;

        question.AddDomainEvent(new QuestionCreated(question));

        return question;
    }

    public ErrorOr<bool> Update(string questionText, string answer)
    {
        base.Update();
        QuestionText = questionText;
        Answer = answer;

        var result = this.Validate();
        if (result.IsError) return result.Errors;

        this.AddDomainEvent(new QuestionUpdated(this));

        return true;
    }

    public void Delete()
    {
        this.AddDomainEvent(new QuestionDeleted(this));
    }

#pragma warning disable CS8618
    private Question()
    {
    }
#pragma warning restore CS8618
}
