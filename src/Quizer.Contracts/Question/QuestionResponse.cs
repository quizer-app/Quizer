namespace Quizer.Contracts.Question;

public record QuestionResponse(
    string Id,
    string Question,
    List<AnswerResponse> Answers
    );