using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Infrastructure.Persistance.Configuration
{
    public class QuizConfigurations : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            ConfigureQuizTable(builder);
            ConfigureQuestionsTable(builder);
        }

        private static void ConfigureQuestionsTable(EntityTypeBuilder<Quiz> builder)
        {
            builder.OwnsMany(q => q.Questions, qb =>
            {
                qb.ToTable("Questions");

                qb.WithOwner().HasForeignKey("QuizId");

                qb.HasKey("Id", "QuizId");

                qb.Property(qs => qs.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("QuestionId")
                    .HasConversion(
                        id => id.Value,
                        value => QuestionId.Create(value)
                        );

                qb.Property(qs => qs.QuestionText)
                    .HasMaxLength(300)
                    .IsRequired();

                qb.Property(qs => qs.Answer)
                    .HasMaxLength(500)
                    .IsRequired();
            });

            builder.Metadata.FindNavigation(nameof(Quiz.Questions))!
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

            builder.Property(q => q.Description)
                .HasMaxLength(1000)
                .IsRequired();

            builder.OwnsOne(q => q.AverageRating);
        }
    }
}
