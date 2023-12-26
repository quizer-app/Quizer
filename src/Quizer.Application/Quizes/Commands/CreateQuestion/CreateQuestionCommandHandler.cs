using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.CreateQuestion;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, ErrorOr<QuestionId>>
{
    private readonly IQuizRepository _quizRepository;

    public CreateQuestionCommandHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<QuestionId>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(QuizId.Create(request.QuizId));

        if (quiz is null)
            return Errors.Quiz.NotFound;

        // TODO: update
        //var result = Question.Create(request.QuestionText, request.Answer);
        //if (result.IsError) return result.Errors;
        //var question = result.Value;

        //quiz.AddQuestion(question);

        //return question.Id;
        
        return QuestionId.CreateUnique();
    }
}
