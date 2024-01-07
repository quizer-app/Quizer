namespace Quizer.Contracts.Question;

public record AnswerRequest(
    string Text,
    bool IsCorrect);
