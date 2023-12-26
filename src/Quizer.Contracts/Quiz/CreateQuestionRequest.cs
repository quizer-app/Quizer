namespace Quizer.Contracts.Quiz;

public record CreateQuestionRequest(
    string Question,
    string Answer);
