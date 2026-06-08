namespace Quizer.Application.Common.Cqrs;

public interface IRequestDispatcher
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
