using Mapster;
using Quizer.Application.Quizes.Commands.CreateQuiz;
using Quizer.Contracts.Quiz;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Entities;

namespace Quizer.Api.Common.Mapping
{
    public class QuizMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateQuizRequest, CreateQuizCommand>();
            config.NewConfig<Quiz, QuizResponse>()
                .Map(dest => dest.Id, src => src.Id.Value)
                .Map(dest => dest.AverageRating, src => src.AverageRating.Value);

            config.NewConfig<Question, QuestionResponse>()
                .Map(dest => dest.Id, src => src.Id.Value);
        }
    }
}
