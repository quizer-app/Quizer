using ErrorOr;
using Quizer.Domain.Common.Models;
using Quizer.Domain.QuestionAggregate.Entities;
using Quizer.Domain.QuestionAggregate.Events;
using Quizer.Domain.QuestionAggregate.Validation;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Domain.QuestionAggregate;

public sealed class Question : AggregateRoot<QuestionId, Guid>
{
    public QuizId QuizId { get; private set; }
    public string QuestionText { get; private set; }
    public IReadOnlyList<Answer> Answers => _answers.AsReadOnly();

    private List<Answer> _answers;

    private Question(
        Guid userId,
        QuestionId id,
        QuizId quizId,
        string questionText,
        List<Answer> answers) : base(id, userId)
    {
        QuizId = quizId;
        QuestionText = questionText;
        _answers = answers;
    }

    private ErrorOr<bool> Validate()
    {
        var validator = new QuestionValidator();
        var validationResult = validator.Validate(this);
        return base.GetValidationErrors(validationResult);
    }

    public static ErrorOr<Question> Create(
        Guid userId,
        QuizId quizId,
        string questionText,
        List<Answer> answers)
    {
        var question = new Question(
            userId,
            QuestionId.CreateUnique(),
            quizId,
            questionText,
            answers);

        var result = question.Validate();
        if (result.IsError) return result.Errors;

        question.AddDomainEvent(new QuestionCreated(question));

        return question;
    }

    public ErrorOr<bool> Update(
        Guid userId,
        string questionText,
        List<Answer> answers)
    {
        base.Update(userId);
        QuestionText = questionText;
        _answers = answers;

        var result = this.Validate();
        if (result.IsError) return result.Errors;

        return true;
    }

    public void Delete()
    {
        this.AddDomainEvent(new QuestionDeleted(this));
    }

#pragma warning disable CS8618
    private Question() {}
#pragma warning restore CS8618
}
