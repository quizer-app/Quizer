using Mapster;
using Quizer.Application.Quizes.Commands.CreateQuestion;
using Quizer.Application.Quizes.Commands.CreateQuiz;
using Quizer.Application.Quizes.Commands.UpdateQuestion;
using Quizer.Application.Quizes.Commands.UpdateQuiz;
using Quizer.Contracts.Quiz;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Api.Common.Mapping;

public class QuizMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        RegisterQuiz(config);
        RegisterQuestion(config);
    }

    private static void RegisterQuestion(TypeAdapterConfig config)
    {
        config.NewConfig<Question, QuestionResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Question, src => src.QuestionText);

        config.NewConfig<(UpdateQuestionRequest Request, Guid QuestionId), UpdateQuestionCommand>()
            .Map(dest => dest.QuestionId, src => src.QuestionId)
            .Map(dest => dest.QuestionText, src => src.Request.Question)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<CreateQuestionRequest, CreateQuestionCommand>()
            .Map(dest => dest.QuestionText, src => src.Question);
    }

    private static void RegisterQuiz(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateQuizRequest Request, string UserId), CreateQuizCommand>()
            .Map(dest => dest.UserId, src => new Guid(src.UserId))
            .Map(dest => dest, src => src.Request);

        config.NewConfig<(UpdateQuizRequest Request, Guid QuizId), UpdateQuizCommand>()
            .Map(dest => dest.QuizId, src => src.QuizId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<QuizId, QuizIdResponse>()
            .Map(dest => dest.Id, src => src.Value.ToString());

        config.NewConfig<Quiz, QuizResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.UserId, src => src.UserId.ToString())
            .Map(dest => dest.AverageRating, src => src.AverageRating.Value)
            .Map(dest => dest.NumberOfRatings, src => src.AverageRating.NumRatings);
    }
}
