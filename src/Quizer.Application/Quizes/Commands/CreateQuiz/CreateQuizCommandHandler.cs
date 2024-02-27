using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Services.Image;
using Quizer.Application.Services.Slugify;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Quizes.Commands.CreateQuiz;

public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, ErrorOr<QuizId>>
{
    private readonly IQuizRepository _quizRepository;
    private readonly ISlugifyService _slugifyService;
    private readonly UserManager<User> _userManager;
    private readonly IImageService _imageService;

    public CreateQuizCommandHandler(IQuizRepository quizRepository, ISlugifyService slugifyService, UserManager<User> userManager, IImageService imageService)
    {
        _quizRepository = quizRepository;
        _slugifyService = slugifyService;
        _userManager = userManager;
        _imageService = imageService;
    }

    public async Task<ErrorOr<QuizId>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        string userName = user is null || user.UserName is null ? "Anonymous" : user.UserName;

        if ((await _quizRepository.Get(userName, request.Name)) is not null)
            return Errors.Quiz.DuplicateName;

        string slug = _slugifyService.GenerateSlug(request.Name);

        bool uploaded = await _imageService.IsSuccessfulyUploaded(request.ImageId);
        if (!uploaded)
            return Errors.Image.CannotUpload;

        var result = Quiz.Create(
            request.UserId,
            Image.CreateNew(request.ImageId, _imageService.GenerateImageUrl(request.ImageId, "quiz")),
            request.Name,
            slug,
            request.Description,
            userName
            );

        if (result.IsError)
            return result.Errors;

        var quiz = result.Value;

        await _quizRepository.Add(quiz);

        return (QuizId)quiz.Id;
    }
}
