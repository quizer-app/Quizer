using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Common.Interfaces.Persistance;

namespace Quizer.Application.Common.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!IsCommand())
            return await next();

        var result = await next();

        if (!result.IsError)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }

    private static bool IsCommand()
    {
        return typeof(ICommand<TResponse>).IsAssignableFrom(typeof(TRequest));
    }
}
