using Quizer.Domain.Common.Models;
using Quizer.Domain.QuizAggregate.Events;
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

    public static Question Create(string questionText, string answer)
    {
        var question = new Question(QuestionId.CreateUnique(), questionText, answer);

        question.AddDomainEvent(new QuestionCreated(question));

        return question;
    }

    public void Update(string questionText, string answer)
    {
        base.Update();
        QuestionText = questionText;
        Answer = answer;

        this.AddDomainEvent(new QuestionUpdated(this));
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
