namespace Quizer.Contracts.Quiz;

public record CreateQuizRequest(
    string Name,
    string Description,
    List<CreateQuestionRequest> Questions
    );
