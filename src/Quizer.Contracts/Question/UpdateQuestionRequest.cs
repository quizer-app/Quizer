namespace Quizer.Contracts.Question;

public record UpdateQuestionRequest(
    string Question,
    string Answer
    );