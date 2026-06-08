using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Users.Common;

namespace Quizer.Application.Users.Queries.GetUser;

public record GetUserQuery(string UserName) : IQuery<ErrorOr<UserResult>>;
