using FluentValidation;
using Quizer.Application.Common.Validation;

namespace Quizer.Application.Quizes.Queries.GetQuizes
{
    public class GetQuizesValidator : AbstractValidator<GetQuizesQuery>
    {
        public GetQuizesValidator()
        {
            RuleFor(q => q.UserId)
                .BeValidGuid()
                .When(q => !string.IsNullOrEmpty(q.UserId));
        }
    }
}
