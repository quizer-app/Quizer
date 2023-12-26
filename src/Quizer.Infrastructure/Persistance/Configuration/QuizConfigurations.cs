using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Infrastructure.Persistance.Configuration;

public class QuizConfigurations : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        ConfigureQuizTable(builder);
        ConfigureQuizQuestionIdsTable(builder);
    }

    private static void ConfigureQuizQuestionIdsTable(EntityTypeBuilder<Quiz> builder)
    {
        builder.OwnsMany(q => q.QuestionIds, qs =>
        {
            qs.ToTable("QuizQuestionIds");

            qs.WithOwner().HasForeignKey("QuizId");

            qs.HasKey("Id");

            qs.Property(i => i.Value)
                .HasColumnName("QuestionId")
                .ValueGeneratedNever();
        });

        builder.Metadata.FindNavigation(nameof(Quiz.QuestionIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureQuizTable(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizes");

        builder.HasKey(q => q.Id);

        builder.HasIndex(q => new { q.UserName, q.Name })
            .IsUnique();

        builder.Property(q => q.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => QuizId.Create(value)
                );

        builder.Property(q => q.Name)
            .HasMaxLength(100);

        builder.Property(q => q.Description)
            .HasMaxLength(1000);

        builder.OwnsOne(q => q.AverageRating);
    }
}
