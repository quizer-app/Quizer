using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Infrastructure.Persistance.Configuration;

public class QuizConfigurations : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        ConfigureQuizTable(builder);
        ConfigureQuestionsTable(builder);
    }

    private static void ConfigureQuestionsTable(EntityTypeBuilder<Quiz> builder)
    {
        builder.OwnsMany(q => q.QuestionIds, qb =>
        {
            qb.ToTable("QuizQuestionIds");

            // TODO: implement + add config for questions
        });

        builder.Metadata.FindNavigation(nameof(Quiz.QuestionIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureQuizTable(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizes");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => QuizId.Create(value)
                );

        builder.Property(q => q.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(q => new { q.UserName, q.Name }).IsUnique();

        builder.Property(q => q.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.OwnsOne(q => q.AverageRating);
    }
}
