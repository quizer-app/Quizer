using Mapster;
using Quizer.Application.Quizes.Commands.CreateQuiz;
using Quizer.Contracts.Quiz;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Api.Common.Mapping
{
    public class QuizMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateQuizRequest, CreateQuizCommand>();
            config.NewConfig<Quiz, QuizResponse>();
        }
    }
}
