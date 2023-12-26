using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Services.Slugify;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Entities;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Quizes.Commands.CreateQuiz;

public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, ErrorOr<QuizId>>
{
    private readonly IQuizRepository _quizRepository;
    private readonly ISlugifyService _slugifyService;
    private readonly UserManager<User> _userManager;

    public CreateQuizCommandHandler(IQuizRepository quizRepository, ISlugifyService slugifyService, UserManager<User> userManager)
    {
        _quizRepository = quizRepository;
        _slugifyService = slugifyService;
        _userManager = userManager;
    }

    public async Task<ErrorOr<QuizId>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        string userName = user is null || user.UserName is null ? "Anonymous" : user.UserName;

        if ((await _quizRepository.Get(userName, request.Name)) is not null)
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

        string slug = _slugifyService.GenerateSlug(request.Name);

        var result = Quiz.Create(
            request.Name,
            slug,
            request.Description,
            request.UserId,
            userName,
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
