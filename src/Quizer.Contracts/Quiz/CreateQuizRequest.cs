namespace Quizer.Contracts.Quiz;

public record CreateQuizRequest(
    string Name,
    string Description,
    List<QuestionRequest> Questions
    );

public record QuestionRequest(
    string Question,
    string Answer);
