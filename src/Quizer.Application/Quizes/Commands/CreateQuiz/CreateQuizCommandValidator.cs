using FluentValidation;

namespace Quizer.Application.Quizes.Commands.CreateQuiz;

public class CreateQuizCommandValidator : AbstractValidator<CreateQuizCommand>
{
    public CreateQuizCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.Questions)
            .NotEmpty();

        RuleForEach(x => x.Questions)
            .NotEmpty()
            .ChildRules(q =>
            {
                q.RuleFor(q => q.QuestionText)
                    .NotEmpty()
                    .MaximumLength(300);

                q.RuleFor(q => q.Answer)
                    .NotEmpty()
                    .MaximumLength(500);
            });
    }
}
