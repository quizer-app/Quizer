using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Common.Interfaces.Persistance
{
    public interface IQuizRepository
    {
        Task Add(Quiz quiz);
        Task<List<Quiz>> GetAll(Guid? userId = null);
        Task<Quiz?> Get(QuizId id);
        Task<Quiz?> Get(string name);
    }
}
