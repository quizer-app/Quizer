using ErrorOr;

namespace Quizer.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "Email is already taken.");

        public static Error DuplicateUsername => Error.Conflict(
            code: "User.DuplicateUsername",
            description: "Username is already taken.");

        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "Username with this username was not found");
    }
}
