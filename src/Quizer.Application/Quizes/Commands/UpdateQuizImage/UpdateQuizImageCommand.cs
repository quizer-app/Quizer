using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.UpdateQuizImage;

public record UpdateQuizImageCommand(
    Guid QuizId,
    IFormFile File) : IRequest<ErrorOr<QuizId>>;