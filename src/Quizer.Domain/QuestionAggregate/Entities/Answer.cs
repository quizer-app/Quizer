using ErrorOr;
using Quizer.Domain.Common.Models;
using Quizer.Domain.QuestionAggregate.ValueObjects;

namespace Quizer.Domain.QuestionAggregate.Entities;

public sealed class Answer : Entity<AnswerId>
{
    public string Text { get; private set; }
    public bool IsCorrect { get; private set; }

    private Answer(
        AnswerId id,
        string answer,
        bool isCorrect) : base(id)
    {
        Text = answer;
        IsCorrect = isCorrect;
    }

    public static ErrorOr<Answer> Create(string answer, bool isCorrect)
    {
        var question = new Answer(AnswerId.CreateUnique(), answer, isCorrect);

        return question;
    }

    public ErrorOr<bool> Update(string answer, bool isCorrect)
    {
        base.Update();
        Text = answer;
        IsCorrect = isCorrect;

        return true;
    }

#pragma warning disable CS8618
    private Answer() {}
#pragma warning restore CS8618
}
