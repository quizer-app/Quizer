namespace Quizer.Contracts.Quiz;

public record UpdateQuizRequest(
    string Name,
    string Description,
    Guid ImageId
    );
