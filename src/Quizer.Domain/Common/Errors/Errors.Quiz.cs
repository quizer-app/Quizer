using ErrorOr;

namespace Quizer.Domain.Common.Errors;

public static partial class Errors
{
    public static class Quiz
    {
        public static Error NotFound => Error.NotFound(
            code: "Quiz.NotFound",
            description: "Quiz was not found");

        public static Error DuplicateName => Error.Conflict(
            code: "Quiz.DuplicateName",
            description: "Quiz with this name already exists");

        public static Error Forbidden => Error.Forbidden(
            code: "Quiz.Forbidden",
            description: "User is not allowed to modify this quiz");

        public static Error AttemptLimitReached => Error.Forbidden(
            code: "Quiz.AttemptLimitReached",
            description: "User has reached the maximum number of attempts for this quiz");

        public static Error TimedConfigurationMissing => Error.Validation(
            code: "Quiz.TimedConfigurationMissing",
            description: "Quiz does not have a time limit configured");
    }
}
