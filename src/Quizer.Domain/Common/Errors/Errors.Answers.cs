using ErrorOr;

namespace Quizer.Domain.Common.Errors;

public static partial class Errors
{
    public static class Answers
    {
        public static Error InvalidId => Error.NotFound(
            code: "Answer.InvalidId",
            description: "Answer with this id does not exist");
    }
}
