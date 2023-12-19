namespace Quizer.Contracts.Quiz
{
    public record CreateQuizRequest(
        string Name,
        string Description,
        List<Question> Questions
        );

    public record Question(
        string QuestionText,
        string Answer);
}
