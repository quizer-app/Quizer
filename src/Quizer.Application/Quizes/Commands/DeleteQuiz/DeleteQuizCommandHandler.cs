using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.DeleteQuiz;

public class DeleteQuizCommandHandler : IRequestHandler<DeleteQuizCommand, ErrorOr<QuizId>>
{
    private readonly IQuizRepository _quizRepository;

    public DeleteQuizCommandHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<QuizId>> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(QuizId.Create(request.QuizId));

        if (quiz is null)
            return Errors.Quiz.NotFound;

        var id = (QuizId)quiz.Id;
        quiz.Delete();
        _quizRepository.Delete(quiz);

        return id;
    }
}
