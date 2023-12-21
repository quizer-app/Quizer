using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizes
{
    public class GetQuizesCommandHandler : IRequestHandler<GetQuizesQuery, ErrorOr<List<Quiz>>>
    {
        public async Task<ErrorOr<List<Quiz>>> Handle(GetQuizesQuery request, CancellationToken cancellationToken)
        {
            return new List<Quiz>();
        }
    }
}
