using ErrorOr;
using Microsoft.Extensions.Logging;
using Quizer.Application.Common.Cqrs;

namespace Quizer.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting request {RequestName}",
            typeof(TRequest).Name);

        var result = await next();

        _logger.LogInformation(
            "Completed request {RequestName} with error={IsError}",
            typeof(TRequest).Name,
            result.IsError);

        return result;
    }
}
