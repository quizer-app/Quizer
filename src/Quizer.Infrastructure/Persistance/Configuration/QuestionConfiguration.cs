using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuestionAggregate.ValueObjects;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Infrastructure.Persistance.Configuration;

public class QuestionConfigurations : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        ConfigureQuestionTable(builder);
        ConfigureAnswersTable(builder);
    }

    private void ConfigureAnswersTable(EntityTypeBuilder<Question> builder)
    {
        builder.OwnsMany(q => q.Answers, ans =>
        {
            ans.ToTable("QuestionAnswers");

            ans.WithOwner().HasForeignKey("QuestionId");

            ans.HasKey("Id", "QuestionId");

            ans.Property(a => a.Id)
                .HasColumnName("AnswerId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => AnswerId.Create(value)
                    );

            ans.Property(a => a.Text)
                .HasMaxLength(200);
        });

        builder.Metadata.FindNavigation(nameof(Question.Answers))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureQuestionTable(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => QuestionId.Create(value)
                );

        builder.Property(q => q.QuizId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => QuizId.Create(value)
                );

        builder.Property(q => q.QuestionText)
            .HasMaxLength(200);

    }
}
