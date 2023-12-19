namespace Quizer.Contracts.Quiz
{
    public record QuizResponse(
        string Id,
        string Name,
        string Description,
        float? AverageRating,
        List<QuestionResponse> Questions
        );

    public record QuestionResponse(
        string Id,
        string QuestionText,
        string Answer
        );
}
