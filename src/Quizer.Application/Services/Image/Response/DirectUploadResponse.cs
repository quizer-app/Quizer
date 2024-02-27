using System.Text.Json.Serialization;

namespace Quizer.Application.Services.Image.Response;

public class DirectUploadResponse
{
    [JsonPropertyName("result")]
    public required Result Result { get; set; }

    [JsonPropertyName("success")]
    public required bool Success { get; set; }

    [JsonPropertyName("errors")]
    public required List<MessageDetails> Errors { get; set; }

    [JsonPropertyName("messages")]
    public required List<MessageDetails> Messages { get; set; } 
}
