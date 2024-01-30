using Quizer.Contracts.Authentication;
using System.Text;
using System.Text.Json;

namespace Quizer.Seed;

internal class Program
{
    private static readonly HttpClient _httpClient = new();

    private static async Task<LoginResponse?> Login(string email, string password)
    {
        var loginRequest = new LoginRequest(email, password, true);

        var stringContent = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/auth/login", stringContent);

        string stringResponse = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonSerializer.Deserialize<LoginResponse>(stringResponse);

        return loginResponse;
    }

    private static void Main(string[] args)
    {
        _httpClient.BaseAddress = new Uri("https://localhost:7131");

        var login = Login("admin@quizer.com", "Test123#");
    }
}
