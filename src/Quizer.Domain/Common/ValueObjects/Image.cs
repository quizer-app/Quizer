using Quizer.Domain.Common.Models;

namespace Quizer.Domain.Common.ValueObjects;

public sealed class Image : ValueObject
{
    public Guid ImageId { get; private set; }
    public Uri Url { get; private set; }

    private Image(Guid imageId, Uri url)
    {
        ImageId = imageId;
        Url = url;
    }

    public static Image CreateNew(Guid imageId, Uri url)
    {
        return new Image(imageId, url);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ImageId;
    }
}
