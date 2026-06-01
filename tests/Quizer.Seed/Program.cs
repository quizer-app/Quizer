using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quizer.Application;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuestionAggregate.Entities;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.UserAggregate;
using Quizer.Infrastructure;
using Quizer.Infrastructure.Persistance;

namespace Quizer.Seed;

internal class Program
{
    private static readonly Guid ImageId = new("91b6de42-f148-4d9e-5062-5bd4d9149600");
    private static readonly Uri ImageUrl = new("https://localhost/images/default-quiz.png");

    private static readonly IReadOnlyList<(string Name, string Description)> QuizTemplates =
    [
        ("Introduction to Science", "Test your basic knowledge of scientific concepts and principles in this introductory quiz."),
        ("World History Trivia", "Explore the depths of history with questions spanning civilizations, events, and influential figures."),
        ("Math Challenge", "Put your math skills to the test with challenging problems and brain-teasers covering various mathematical topics."),
        ("Literature Classics", "Delve into the world of literature with questions about famous authors, novels, and literary movements."),
        ("Geography Explorer", "Embark on a journey around the globe as you answer questions about countries, capitals, and geographic features."),
        ("Music Trivia Madness", "From classical compositions to modern hits, this quiz covers a wide range of musical genres and artists."),
        ("Movie Buff Challenge", "Test your knowledge of cinema with questions about iconic films, directors, actors, and memorable quotes."),
        ("Sports Fanatic Showdown", "Put on your sports cap and tackle questions about your favorite teams, athletes, and sporting events."),
    ];

    private static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        var repositoryRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var apiSettingsDirectory = Path.Combine(repositoryRoot, "src", "Quizer.Api");

        builder.Configuration.SetBasePath(apiSettingsDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables();

        builder.Services
            .AddApplication(builder.Configuration)
            .AddInfractructure(builder.Configuration);

        using var host = builder.Build();
        await using var scope = host.Services.CreateAsyncScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var context = scope.ServiceProvider.GetRequiredService<QuizerDbContext>();

        var adminUser = await userManager.FindByEmailAsync("admin@quizer.com");
        if (adminUser is null || string.IsNullOrWhiteSpace(adminUser.UserName) || !Guid.TryParse(adminUser.Id, out var adminUserId))
        {
            Console.WriteLine("Admin user not found. Run API migrations/seeding first.");
            return;
        }

        foreach (var template in QuizTemplates)
        {
            var exists = await context.Quizes.AnyAsync(q => q.UserName == adminUser.UserName && q.Name == template.Name);
            if (exists)
            {
                continue;
            }

            var quizResult = Quiz.Create(
                userId: adminUserId,
                image: Image.CreateNew(ImageId, ImageUrl),
                name: template.Name,
                slug: template.Name.ToLowerInvariant().Replace(' ', '-'),
                description: template.Description,
                userName: adminUser.UserName,
                timeLimitInSeconds: null,
                autoSubmitWhenTimeExpires: false,
                maxAttemptsPerUser: null);

            if (quizResult.IsError)
            {
                continue;
            }

            var quiz = quizResult.Value;
            context.Quizes.Add(quiz);

            for (var i = 1; i <= 10; i++)
            {
                var answerResult = Answer.Create($"Answer to question number {i}", true);
                if (answerResult.IsError)
                {
                    continue;
                }

                var questionResult = Question.Create(
                    adminUserId,
                    (QuizId)quiz.Id,
                    $"Question number {i}",
                    [answerResult.Value]);

                if (questionResult.IsError)
                {
                    continue;
                }

                var question = questionResult.Value;
                quiz.AddQuestion(adminUserId, question);
                context.Questions.Add(question);
            }
        }

        await context.SaveChangesAsync();
    }
}
