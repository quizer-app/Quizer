using Quizer.Contracts.Question;

namespace Quizer.Contracts.Quiz;

public record QuizResponse(
    string Id,
    string UserName,
    string Name,
    string Slug,
    string Location,
    string Description,
    double AverageRating,
    int NumberOfRatings,
    int? TimeLimitInSeconds,
    bool AutoSubmitWhenTimeExpires,
    int? MaxAttemptsPerUser,
    IReadOnlyList<QuestionResponse> Questions,
    string CreatedBy,
    string UpdatedBy,
    string ImageUrl,
    string ImageId,
    DateTime CreatedAt,
    DateTime UpdatedAt);
