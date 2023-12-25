using MediatR;
using Quizer.Domain.QuizAggregate.Events;

namespace Quizer.Application.Quizes.Events;

public class QuizCreatedHandler : INotificationHandler<QuizCreated>
{
    public Task Handle(QuizCreated notification, CancellationToken cancellationToken)
    {
        // TODO: implement
        return Task.CompletedTask;
    }
}
