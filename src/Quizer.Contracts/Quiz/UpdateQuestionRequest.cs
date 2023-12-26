namespace Quizer.Contracts.Quiz;

public record UpdateQuestionRequest(
    string Question,
    string Answer
    );