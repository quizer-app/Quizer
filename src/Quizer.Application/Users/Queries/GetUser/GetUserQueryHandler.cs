using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Microsoft.AspNetCore.Identity;
using Quizer.Application.Users.Common;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Users.Queries.GetUser;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, ErrorOr<UserResult>>
{
    private readonly UserManager<User> _userManager;

    public GetUserQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<UserResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null) return Errors.User.NotFound;

        return new UserResult(
            new Guid(user.Id),
            user.UserName!,
            user.Email!,
            user.CreatedAt);
    }
}