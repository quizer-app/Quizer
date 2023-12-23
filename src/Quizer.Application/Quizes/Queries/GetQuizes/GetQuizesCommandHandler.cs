using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizes
{
    public class GetQuizesCommandHandler : IRequestHandler<GetQuizesQuery, ErrorOr<List<Quiz>>>
    {
        private readonly IQuizRepository _quizRepository;

        public GetQuizesCommandHandler(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<ErrorOr<List<Quiz>>> Handle(GetQuizesQuery request, CancellationToken cancellationToken)
        {
            Guid? userId = request.UserId is null ? null : new Guid(request.UserId);
            return await _quizRepository.GetAll(userId);
        }
    }
}
