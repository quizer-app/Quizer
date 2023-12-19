using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Common.Interfaces.Persistance
{
    public interface IQuizRepository
    {
        Task Add(Quiz quiz);
    }
}
