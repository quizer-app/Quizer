namespace Quizer.Application.Quizes.Queries.GetQuizImage;

public record QuizImageResponse(
    byte[] ImageData,
    string Mime);
