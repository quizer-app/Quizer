using Mapster;
using Quizer.Application.Quizes.Commands.CreateQuiz;
using Quizer.Application.Quizes.Commands.UpdateQuiz;
using Quizer.Contracts.Quiz;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Entities;

namespace Quizer.Api.Common.Mapping;

public class QuizMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateQuizRequest Request, string UserId), CreateQuizCommand>()
            .Map(dest => dest.UserId, src => new Guid(src.UserId))
            .Map(dest => dest, src => src.Request);

        config.NewConfig<(UpdateQuizRequest Request, Guid QuizId), UpdateQuizCommand>()
            .Map(dest => dest.QuizId, src => src.QuizId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<QuestionRequest, QuestionCommand>()
            .Map(dest => dest.QuestionText, src => src.Question);

        config.NewConfig<Quiz, QuizResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.UserId, src => src.UserId.ToString())
            .Map(dest => dest.AverageRating, src => src.AverageRating.Value)
            .Map(dest => dest.NumberOfRatings, src => src.AverageRating.NumRatings);

        config.NewConfig<Question, QuestionResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Question, src => src.QuestionText);

        config.NewConfig<QuizId, QuizIdResponse>()
            .Map(dest => dest.Id, src => src.Value.ToString());
    }
}
