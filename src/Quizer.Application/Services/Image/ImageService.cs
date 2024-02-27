using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quizer.Application.Common.Settings;
using Quizer.Application.Services.Image.Response;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace Quizer.Application.Services.Image;

public class ImageService : IImageService
{
    private readonly ILogger<ImageService> _logger;
    private readonly HttpClient _client;

    public ImageService(ILogger<ImageService> logger, HttpClient client, IOptions<ImagesSettings> imageOptions)
    {
        var settings = imageOptions.Value;
        _logger = logger;
        _client = client;

        _client.BaseAddress = new Uri($"https://api.cloudflare.com/client/v4/accounts/{settings.AccountId}/images/v2/");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiToken);
    }

    public async Task<ErrorOr<DirectUploadResponse>> DirectUpload(bool requireSignedURLs = false)
    {
        var formData = new MultipartFormDataContent
        {
            { new StringContent(requireSignedURLs.ToString().ToLower()), "requireSignedURLs" }
        };

        var response = await _client.PostAsync("direct_upload", formData);

        _logger.LogInformation("Direct upload response: {@response}", response);

        var failureError = Error.Failure(
                code: "Images.UploadFailed",
                description: "Failed to get direct upload URL");
        if (!response.IsSuccessStatusCode)
            return failureError;

        var content = await response.Content.ReadAsStringAsync();
        if(content is null)
            return failureError;

        var result = JsonSerializer.Deserialize<DirectUploadResponse>(content);
        if (result is null || !result.Success)
            return failureError;

        return result;
    }
}
