﻿using System.Text.Json.Serialization;

namespace Quizer.Application.Services.Image.Response;

public class MessageDetails
{
    [JsonPropertyName("code")]
    public required int Code { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }
}
