using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Entities;
using Quizer.Domain.UserAggregate;

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
                new Guid(request.UserId),
                AverageRating.CreateNew(),
                request.Questions
                    .ConvertAll(q => Question.Create(
                        q.QuestionText,
                        q.Answer)));

            await _quizRepository.Add(quiz);

            return quiz;
        }
    }
}
