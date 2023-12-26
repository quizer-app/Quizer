using MediatR;
using Quizer.Domain.QuizAggregate.Events;

namespace Quizer.Application.Quizes.Events;

public class QuizCreatedHandler : INotificationHandler<QuizDeleted>
{
    public Task Handle(QuizDeleted notification, CancellationToken cancellationToken)
    {
        // TODO: implement
        return Task.CompletedTask;
    }
}
