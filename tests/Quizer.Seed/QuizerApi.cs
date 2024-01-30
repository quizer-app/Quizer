using Quizer.Contracts.Authentication;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Quizer.Contracts.Quiz;
using Quizer.Contracts.Question;

namespace Quizer.Seed;

public class QuizerApi
{
    private static readonly HttpClient _httpClient = CreateHttpClient(_httpClient);

    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private static HttpClient CreateHttpClient(HttpClient? client)
    {
        client = new HttpClient();
        client.BaseAddress = new Uri("https://api.local.elotoja.com");
        return client;
    }

    public async Task<LoginResponse?> Login(LoginRequest request)
    {
        var stringContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/Auth/login", stringContent);

        if(!response.IsSuccessStatusCode)
            return null;

        string stringResponse = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonSerializer.Deserialize<LoginResponse>(stringResponse, _serializerOptions);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse!.AccessToken);

        return loginResponse;
    }

    public async Task<QuizIdResponse?> AddQuiz(CreateQuizRequest request)
    {
        var stringContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/Quiz", stringContent);

        if (!response.IsSuccessStatusCode)
            return null;

        string stringResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<QuizIdResponse>(stringResponse, _serializerOptions);
    }

    public async Task<QuestionIdResponse?> AddQuestion(CreateQuestionRequest request, string quizId)
    {
        var stringContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"/api/v1/Question/{quizId}", stringContent);

        if (!response.IsSuccessStatusCode)
            return null;

        string stringResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<QuestionIdResponse>(stringResponse, _serializerOptions);
    }
}
