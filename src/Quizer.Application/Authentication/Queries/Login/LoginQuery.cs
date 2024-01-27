using ErrorOr;
using MediatR;

namespace Quizer.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
    ) : IRequest<ErrorOr<LoginResult>>;
