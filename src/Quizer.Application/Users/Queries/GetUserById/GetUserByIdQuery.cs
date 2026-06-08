using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Users.Common;

namespace Quizer.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IQuery<ErrorOr<UserResult>>;
