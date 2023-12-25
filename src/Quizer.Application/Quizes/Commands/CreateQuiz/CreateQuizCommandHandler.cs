using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Entities;

namespace Quizer.Application.Quizes.Commands.CreateQuiz;

public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, ErrorOr<QuizId>>
{
    private readonly IQuizRepository _quizRepository;

    public CreateQuizCommandHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<QuizId>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        if ((await _quizRepository.Get(request.Name)) is not null)
            return Errors.Quiz.DuplicateName;

        var errors = new List<Error>();

        var questionResults = request.Questions
                .ConvertAll(q => Question.Create(
                    q.QuestionText,
                    q.Answer));

        foreach (var questionResult in questionResults)
            if (questionResult.IsError)
                errors.AddRange(questionResult.Errors);

        var questions = questionResults
            .ConvertAll(q => q.Value);

        var result = Quiz.Create(
            request.Name,
            request.Description,
            request.UserId,
            AverageRating.CreateNew(),
            questions
            );

        if (result.IsError)
            errors.AddRange(result.Errors);

        if(errors.Any())
            return errors;

        var quiz = result.Value;

        await _quizRepository.Add(quiz);

        return (QuizId)quiz.Id;
    }
}
