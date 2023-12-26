using ErrorOr;

namespace Quizer.Domain.Common.Errors;

public static partial class Errors
{
    public static class Question
    {
        public static Error NotFound => Error.NotFound(
            code: "Question.NotFound",
            description: "Question was not found");
    }
}
