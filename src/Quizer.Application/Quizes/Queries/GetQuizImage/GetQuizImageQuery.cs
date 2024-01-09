using ErrorOr;
using MediatR;

namespace Quizer.Application.Quizes.Queries.GetQuizImage;

public record GetQuizImageQuery(Guid QuizId
    ) : IRequest<ErrorOr<QuizImageResponse>>;
