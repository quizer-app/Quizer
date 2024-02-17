using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Utils;
using Quizer.Domain.QuizAggregate;
using System.Linq.Expressions;

namespace Quizer.Application.Quizes.Queries.GetQuizes;

public class GetQuizesQueryHandler : IRequestHandler<GetQuizesQuery, ErrorOr<PagedList<Quiz>>>
{
    private readonly IQuizRepository _quizRepository;

    public GetQuizesQueryHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<PagedList<Quiz>>> Handle(GetQuizesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Quiz> quizesQuery = _quizRepository.Quizes;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            quizesQuery = quizesQuery.Where(q =>
                q.Name.Contains(request.SearchTerm) ||
                q.UserName.Contains(request.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(request.UserName))
        {
            quizesQuery = quizesQuery.Where(q => q.UserName == request.UserName);
        }

        Expression<Func<Quiz, object>> keySelector = request.SortColumn?.ToLower() switch
        {
            "name" => q => q.Name,
            "userName" => q => q.UserName,
            "createdAt" => q => q.CreatedAt,
            "averageRating" => q => q.AverageRating,
            "numberOfQuestions" => q => q.NumberOfQuestions,
            _ => q => q.CreatedAt
        };

        if (request.SortOrder?.ToLower() == "desc")
        {
            quizesQuery = quizesQuery.OrderByDescending(keySelector);
        }
        else
        {
            quizesQuery = quizesQuery.OrderBy(keySelector);
        }

        var quizes = await PagedList<Quiz>.CreateAsync(quizesQuery, request.PageNumber, request.PageSize);
        return quizes;
    }
}
