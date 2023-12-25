using ErrorOr;
using MediatR;

namespace Quizer.Application.Authentication.Queries;

public record LoginQuery(
    string Email,
    string Password
    ) : IRequest<ErrorOr<LoginResult>>;
