using FluentValidation;
using Quizer.Domain.QuizAggregate.Entities;

namespace Quizer.Domain.QuizAggregate.Validation;

public class QuestionValidator : AbstractValidator<Question>
{
    public QuestionValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.Answer)
            .NotEmpty()
            .MaximumLength(500);
    }
}
