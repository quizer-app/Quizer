using System.Text.Json.Serialization;

namespace Quizer.Application.Services.Image.Response;

public class Result
{
    [JsonPropertyName("id")]
    public required Guid Id { get; set; }

    [JsonPropertyName("uploadURL")]
    public required Uri UploadUrl { get; set; }
}

