using Mapster;
using Quizer.Application.Quizes.Commands.CreateQuestion;
using Quizer.Application.Quizes.Commands.CreateQuiz;
using Quizer.Application.Quizes.Commands.UpdateQuestion;
using Quizer.Application.Quizes.Commands.UpdateQuiz;
using Quizer.Contracts.Quiz;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuestionAggregate;
using Quizer.Contracts.Question;
using Quizer.Application.Quizes.Common;
using Quizer.Domain.QuestionAggregate.Entities;

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
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.Question, src => src.QuestionText)
            .Map(dest => dest.Answers, src => src.Answers);

        config.NewConfig<Answer, AnswerResponse>()
            .Map(dest => dest, src => src);

        config.NewConfig<(UpdateQuestionRequest Request, Guid QuestionId), UpdateQuestionCommand>()
            .Map(dest => dest.QuestionId, src => src.QuestionId)
            .Map(dest => dest.QuestionText, src => src.Request.Question)
            .Map(dest => dest.Answers, src => src.Request.Answers)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<(CreateQuestionRequest Request, Guid QuizId), CreateQuestionCommand>()
            .Map(dest => dest.QuizId, src => src.QuizId)
            .Map(dest => dest.QuestionText, src => src.Request.Question)
            .Map(dest => dest.Answers, src => src.Request.Answers)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<CreateQuestionAnswer, AnswerRequest>()
            .Map(dest => dest, src => src);

        config.NewConfig<QuestionId, QuestionIdResponse>()
            .Map(dest => dest.Id, src => src.Value.ToString());
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

        config.NewConfig<Quiz, ShortQuizResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.UserId, src => src.UserId.ToString())
            .Map(dest => dest.AverageRating, src => src.AverageRating.Value)
            .Map(dest => dest.NumberOfRatings, src => src.AverageRating.NumRatings);

        config.NewConfig<DetailedQuizResult, QuizResponse>()
            .Map(dest => dest.Id, src => src.Quiz.Id.Value.ToString())
            .Map(dest => dest.UserId, src => src.Quiz.UserId.ToString())
            .Map(dest => dest.AverageRating, src => src.Quiz.AverageRating.Value)
            .Map(dest => dest.NumberOfRatings, src => src.Quiz.AverageRating.NumRatings)
            .Map(dest => dest, src => src.Quiz)
            .Map(dest => dest.Questions, src => src.Questions);
    }
}
