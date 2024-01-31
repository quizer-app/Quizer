using ErrorOr;
using MediatR;
using Quizer.Application.Users.Common;

namespace Quizer.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<ErrorOr<UserResult>>;
