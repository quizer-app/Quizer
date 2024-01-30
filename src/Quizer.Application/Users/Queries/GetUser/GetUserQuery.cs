using ErrorOr;
using MediatR;
using Quizer.Application.Users.Common;

namespace Quizer.Application.Users.Queries.GetUser;

public record GetUserQuery(string UserName) : IRequest<ErrorOr<UserResult>>;
