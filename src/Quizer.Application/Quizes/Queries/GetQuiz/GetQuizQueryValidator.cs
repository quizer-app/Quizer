using FluentValidation;
using Quizer.Application.Common.Validation;

namespace Quizer.Application.Quizes.Queries.GetQuiz
{
    public class GetQuizQueryValidator : AbstractValidator<GetQuizQuery>
    {
        public GetQuizQueryValidator()
        {
            RuleFor(q => q.QuizId)
                .BeValidGuid()
                .When(q => !string.IsNullOrEmpty(q.QuizId));
        }
    }
}
