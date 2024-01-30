namespace Quizer.Contracts.Quiz;

public record ShortQuizResponse(
    string Id,
    string UserName,
    string Name,
    string Slug,
    string Location,
    string Description,
    double AverageRating,
    int NumberOfRatings,
    string CreatedBy,
    string UpdatedBy,
    int NumberOfQuestions,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );
