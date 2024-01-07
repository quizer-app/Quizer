using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Common;

public record DetailedQuizResult(
    Quiz Quiz,
    IReadOnlyList<Question> Questions);