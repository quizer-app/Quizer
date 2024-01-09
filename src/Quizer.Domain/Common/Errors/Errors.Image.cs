using ErrorOr;

namespace Quizer.Domain.Common.Errors;

public static partial class Errors
{
    public static class Image
    {
        public static Error CannotUpload => Error.Failure(
            code: "Image.CannotUpload",
            description: "There was an error during image upload");

        public static Error WrongFormat(string[] formats) => Error.Validation(
            code: "Image.WrongFormat",
            description: $"Image has wrong format. Accepted formats: {string.Join(", ", formats)}");
    }
}
