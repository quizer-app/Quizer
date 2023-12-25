namespace Quizer.Contracts.Quiz;

public record QuizResponse(
    string Id,
    string UserId,
    string Name,
    string Description,
    double AverageRating,
    IReadOnlyList<QuestionResponse> Questions
    );

public record QuestionResponse(
    string Id,
    string Question,
    string Answer
    );
