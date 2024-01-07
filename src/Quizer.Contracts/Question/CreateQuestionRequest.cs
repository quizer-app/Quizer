namespace Quizer.Contracts.Question;

public record CreateQuestionRequest(
    string Question,
    List<AnswerRequest> Answers);
