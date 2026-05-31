using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quizer.Infrastructure.Persistance;
using Xunit;

namespace Quizer.Api.IntegrationTests;

public class QuizEndpointsTests : IClassFixture<QuizerApiFactory>
{
    private readonly HttpClient _client;

    public QuizEndpointsTests(QuizerApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_quizes_returns_ok_for_anonymous_request()
    {
        var response = await _client.GetAsync("/api/v1/Quiz?pageNumber=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_quiz_without_token_returns_unauthorized()
    {
        var payload = """
        {
          "name": "Cloud Fundamentals",
          "description": "Intro quiz",
          "imageId": "a5e8433f-6b7f-4a93-bc33-b57f663f4ce2"
        }
        """;
        using var request = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/v1/Quiz", request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

public class QuizerApiFactory : WebApplicationFactory<Program>
{
    public QuizerApiFactory()
    {
        Environment.SetEnvironmentVariable("JwtSettings__Issuer", "test-issuer");
        Environment.SetEnvironmentVariable("JwtSettings__Audience", "test-audience");
        Environment.SetEnvironmentVariable("JwtSettings__Secret", "this-is-a-very-long-test-secret-key-123456");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");


        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<QuizerDbContext>>();
            services.RemoveAll<QuizerDbContext>();
            services.RemoveAll<IDbContextOptionsConfiguration<QuizerDbContext>>();

            services.AddDbContext<QuizerDbContext>(options =>
            {
                options.UseInMemoryDatabase("quizer-api-tests");
                options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        });
    }
}
