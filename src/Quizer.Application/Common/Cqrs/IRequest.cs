namespace Quizer.Application.Common.Cqrs;

public interface IRequest<TResponse>
{
}

public interface ICommand<TResponse> : IRequest<TResponse>
{
}

public interface IQuery<TResponse> : IRequest<TResponse>
{
}
