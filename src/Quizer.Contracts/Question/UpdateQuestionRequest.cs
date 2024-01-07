namespace Quizer.Contracts.Question;

public record UpdateQuestionRequest(
    string Question,
    List<AnswerRequest> Answers);