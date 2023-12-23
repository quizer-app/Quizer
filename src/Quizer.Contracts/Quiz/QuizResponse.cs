namespace Quizer.Contracts.Quiz
{
    public record QuizResponse(
        string Id,
        string Name,
        string Description,
        double AverageRating,
        IReadOnlyList<QuestionResponse> Questions
        );

    public record QuestionResponse(
        string Id,
        string QuestionText,
        string Answer
        );
}
