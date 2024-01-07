namespace Quizer.Contracts.Quiz;

public record ShortQuizResponse(
    string Id,
    string UserId,
    string UserName,
    string Name,
    string Slug,
    string Description,
    double AverageRating,
    int NumberOfRatings
    );
