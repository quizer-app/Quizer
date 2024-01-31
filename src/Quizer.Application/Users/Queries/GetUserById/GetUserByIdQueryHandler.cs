using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Quizer.Application.Users.Common;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<UserResult>>
{
    private readonly UserManager<User> _userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<UserResult>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null) return Errors.User.NotFound;

        return new UserResult(
            new Guid(user.Id),
            user.UserName!,
            user.Email!,
            user.CreatedAt);
    }
}