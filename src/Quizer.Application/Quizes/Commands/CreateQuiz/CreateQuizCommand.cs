using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.CreateQuiz
{
    public record CreateQuizCommand(
        string Name,
        string Description,
        List<QuestionCommand> Questions
        ) : IRequest<ErrorOr<Quiz>>;

    public record QuestionCommand(
        string QuestionText,
        string Answer);
}
