using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Commands.Register;

public record RegisterResult(
    User User
    );
