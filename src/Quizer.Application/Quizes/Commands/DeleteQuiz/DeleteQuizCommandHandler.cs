using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;

namespace Quizer.Application.Quizes.Commands.DeleteQuiz
{
    public class DeleteQuizCommandHandler : IRequestHandler<DeleteQuizCommand, ErrorOr<bool>>
    {
        private readonly IQuizRepository _quizRepository;

        public DeleteQuizCommandHandler(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<ErrorOr<bool>> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
        {
            bool deleted = await _quizRepository.Delete(request.QuizId);

            if (!deleted)
                return Errors.Quiz.NotFound;

            return true;
        }
    }
}
