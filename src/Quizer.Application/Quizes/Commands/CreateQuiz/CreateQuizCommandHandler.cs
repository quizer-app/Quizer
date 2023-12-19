using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Entities;

namespace Quizer.Application.Quizes.Commands.CreateQuiz
{
    public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, ErrorOr<Quiz>>
    {
        private readonly IQuizRepository _quizRepository;

        public CreateQuizCommandHandler(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<ErrorOr<Quiz>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = Quiz.Create(
                request.Name,
                request.Description,
                0,
                request.Questions
                    .ConvertAll(q => Question.Create(
                        q.QuestionText,
                        q.Answer)));

            //await _quizRepository.Add(quiz);

            return quiz;
        }
    }
}
