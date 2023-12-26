namespace Quizer.Contracts.Quiz;

public record QuestionResponse(
    string Id,
    string Question,
    string Answer
    );