using Quizer.Domain.Common.Models;

namespace Quizer.Application.Common.Cqrs;

public interface IDomainEventPublisher
{
    Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}

public interface IDomainEventHandler<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
}
