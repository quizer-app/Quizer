using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Quizer.Application.Common.Cqrs;

public sealed class RequestDispatcher : IRequestDispatcher
{
    private static readonly MethodInfo DispatchCoreMethod = typeof(RequestDispatcher)
        .GetMethod(nameof(DispatchCore), BindingFlags.Instance | BindingFlags.NonPublic)!;

    private readonly IServiceProvider _serviceProvider;

    public RequestDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var dispatchMethod = DispatchCoreMethod.MakeGenericMethod(request.GetType(), typeof(TResponse));

        try
        {
            return (Task<TResponse>)dispatchMethod.Invoke(this, [request, cancellationToken])!;
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            throw exception.InnerException;
        }
    }

    private Task<TResponse> DispatchCore<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        var handler = _serviceProvider.GetService<IRequestHandler<TRequest, TResponse>>();

        if (handler is null)
        {
            throw new InvalidOperationException(
                $"No request handler registered for request type '{typeof(TRequest).FullName}' and response type '{typeof(TResponse).FullName}'.");
        }

        RequestHandlerDelegate<TResponse> next = () => handler.Handle(request, cancellationToken);
        var behaviors = _serviceProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>().Reverse();

        foreach (var behavior in behaviors)
        {
            var current = next;
            next = () => behavior.Handle(request, current, cancellationToken);
        }

        return next();
    }
}
