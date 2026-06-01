using FluentValidation;

namespace Quizer.Domain.QuizAggregate.Validation;

public class QuizValidator : AbstractValidator<Quiz>
{
    public QuizValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.TimeLimitInSeconds)
            .GreaterThan(0)
            .When(x => x.TimeLimitInSeconds.HasValue);

        RuleFor(x => x.MaxAttemptsPerUser)
            .GreaterThan(0)
            .When(x => x.MaxAttemptsPerUser.HasValue);

        RuleFor(x => x.AutoSubmitWhenTimeExpires)
            .Equal(false)
            .When(x => !x.TimeLimitInSeconds.HasValue)
            .WithMessage("Auto submit requires a time limit.");
    }
}
