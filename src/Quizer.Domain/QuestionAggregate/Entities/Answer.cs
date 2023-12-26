using ErrorOr;
using Quizer.Domain.Common.Models;
using Quizer.Domain.QuestionAggregate.ValueObjects;

namespace Quizer.Domain.QuestionAggregate.Entities;

public sealed class Answer : Entity<AnswerId>
{
    public string Text { get; private set; }

    private Answer(AnswerId id, string answer) : base(id)
    {
        Text = answer;
    }

    public static ErrorOr<Answer> Create(string answer)
    {
        var question = new Answer(AnswerId.CreateUnique(), answer);

        return question;
    }

    public ErrorOr<bool> Update(string answer)
    {
        base.Update();
        Text = answer;

        return true;
    }

#pragma warning disable CS8618
    private Answer()
    {
    }
#pragma warning restore CS8618
}
