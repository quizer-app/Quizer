using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuestionAggregate.Entities;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.UserAggregate;
using Quizer.Infrastructure.Authentication;
using Quizer.Infrastructure.Persistance;
using System.Security.Claims;

namespace Quizer.Infrastructure;

public static class MigrationExtensions
{
    public static async Task MigrateDatabase(this IApplicationBuilder app, bool enableSeedData)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<QuizerDbContext>();

        if (context.Database.IsRelational())
        {
            context.Database.Migrate();
        }
        else
        {
            context.Database.EnsureCreated();
        }

        if (!enableSeedData)
        {
            return;
        }

        await app.SeedUsers();
        await app.SeedDevelopmentData();
    }

    public static async Task SeedUsers(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser is null)
        {
            adminUser = new User
            {
                UserName = "admin",
                Email = "admin@quizer.com",
            };
            await userManager.CreateAsync(adminUser, "Test123#");
        }

        var adminRole = await roleManager.FindByNameAsync("Administrator");
        if (adminRole is null)
        {
            adminRole = new IdentityRole("Administrator");
            await roleManager.CreateAsync(adminRole);

            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaims.Permission, Permission.Admin.ToString()));
        }

        if (adminRole.Name is not null && !await userManager.IsInRoleAsync(adminUser, adminRole.Name))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole.Name);
        }
    }

    public static async Task SeedDevelopmentData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<QuizerDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var existingSeededQuiz = await context.Quizes
            .AsNoTracking()
            .AnyAsync(quiz => quiz.UserName == "admin");
        if (existingSeededQuiz)
        {
            return;
        }

        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser is null)
        {
            return;
        }

        if (!Guid.TryParse(adminUser.Id, out var adminUserId))
        {
            return;
        }

        var image = Image.CreateNew(
            Guid.Parse("91b6de42-f148-4d9e-5062-5bd4d9149600"),
            new Uri("https://localhost/seed/quiz-image"));

        var quizDefinitions = new[]
        {
            ("Introduction to Science", "Test your basic knowledge of scientific concepts and principles in this introductory quiz."),
            ("World History Trivia", "Explore the depths of history with questions spanning civilizations, events, and influential figures."),
            ("Math Challenge", "Put your math skills to the test with challenging problems and brain-teasers covering various mathematical topics."),
            ("Literature Classics", "Delve into the world of literature with questions about famous authors, novels, and literary movements."),
            ("Geography Explorer", "Embark on a journey around the globe as you answer questions about countries, capitals, and geographic features."),
            ("Music Trivia Madness", "From classical compositions to modern hits, this quiz covers a wide range of musical genres and artists."),
            ("Movie Buff Challenge", "Test your knowledge of cinema with questions about iconic films, directors, actors, and memorable quotes."),
            ("Sports Fanatic Showdown", "Put on your sports cap and tackle questions about your favorite teams, athletes, and sporting events."),
        };

        foreach (var quizDefinition in quizDefinitions)
        {
            var slug = quizDefinition.Item1
                .ToLowerInvariant()
                .Replace(' ', '-');

            var quizResult = Quiz.Create(
                adminUserId,
                image,
                quizDefinition.Item1,
                slug,
                quizDefinition.Item2,
                adminUser.UserName ?? "admin",
                null,
                false,
                null);
            if (quizResult.IsError)
            {
                continue;
            }

            var quiz = quizResult.Value;

            for (var questionNumber = 1; questionNumber <= 10; questionNumber++)
            {
                var answerResult = Answer.Create($"Answer to question number {questionNumber}", true);
                if (answerResult.IsError)
                {
                    continue;
                }

                var questionResult = Question.Create(
                    adminUserId,
                    (QuizId)quiz.Id,
                    $"Question number {questionNumber}",
                    [answerResult.Value]);
                if (questionResult.IsError)
                {
                    continue;
                }

                var question = questionResult.Value;

                quiz.AddQuestion(adminUserId, question);
                context.Questions.Add(question);
            }

            context.Quizes.Add(quiz);
        }

        await context.SaveChangesAsync();
    }
}
