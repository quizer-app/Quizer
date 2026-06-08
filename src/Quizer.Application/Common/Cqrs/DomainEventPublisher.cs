using Microsoft.Extensions.DependencyInjection;
using Quizer.Domain.Common.Models;
using System.Reflection;

namespace Quizer.Application.Common.Cqrs;

public sealed class DomainEventPublisher : IDomainEventPublisher
{
    private static readonly MethodInfo PublishCoreMethod = typeof(DomainEventPublisher)
        .GetMethod(nameof(PublishCore), BindingFlags.Instance | BindingFlags.NonPublic)!;

    private readonly IServiceProvider _serviceProvider;

    public DomainEventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        var publishMethod = PublishCoreMethod.MakeGenericMethod(domainEvent.GetType());

        try
        {
            return (Task)publishMethod.Invoke(this, [domainEvent, cancellationToken])!;
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            throw exception.InnerException;
        }
    }

    private async Task PublishCore<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken)
        where TDomainEvent : IDomainEvent
    {
        var handlers = _serviceProvider.GetServices<IDomainEventHandler<TDomainEvent>>();

        foreach (var handler in handlers)
        {
            await handler.Handle(domainEvent, cancellationToken);
        }
    }
}
