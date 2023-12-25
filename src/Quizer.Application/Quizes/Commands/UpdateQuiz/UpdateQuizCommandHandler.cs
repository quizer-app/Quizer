using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Quizes.Commands.UpdateQuiz;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.DeleteQuiz;

public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, ErrorOr<QuizId>>
{
    private readonly IQuizRepository _quizRepository;

    public UpdateQuizCommandHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<QuizId>> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(QuizId.Create(request.QuizId));

        if (quiz is null)
            return Errors.Quiz.NotFound;

        var id = (QuizId)quiz.Id;
        quiz.Update(request.Name, request.Description);
        
        return id;
    }
}
