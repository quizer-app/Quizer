using FluentValidation;

namespace Quizer.Application.Quizes.Queries.GetQuizes;

public class GetQuizesValidator : AbstractValidator<GetQuizesQuery>
{
    public GetQuizesValidator()
    {
        RuleFor(q => q.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);

        RuleFor(q => q.PageNumber)
            .GreaterThanOrEqualTo(1);
    }
}
