using System.Linq;
using FluentValidation;

namespace Quizer.Domain.QuestionAggregate.Validation;

public class QuestionValidator : AbstractValidator<Question>
{
    public QuestionValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.Answers)
            .NotEmpty()
            .Must(answers => answers.Any(answer => answer.IsCorrect))
            .WithMessage("At least one answer must be marked as correct.");
    }
}
