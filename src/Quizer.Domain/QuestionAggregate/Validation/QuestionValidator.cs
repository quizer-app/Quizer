using FluentValidation;

namespace Quizer.Domain.QuestionAggregate.Validation;

public class QuestionValidator : AbstractValidator<Question>
{
    public QuestionValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty()
            .MaximumLength(300);
    }
}
