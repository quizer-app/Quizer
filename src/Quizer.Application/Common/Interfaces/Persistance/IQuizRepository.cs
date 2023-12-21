using Quizer.Domain.QuizAggregate;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Common.Interfaces.Persistance
{
    public interface IQuizRepository
    {
        Task Add(Quiz quiz);
        Task<List<Quiz>> GetAll(UserId? userId = null);
        Task<Quiz?> Get(QuizId id);
        Task<Quiz?> Get(string name);
    }
}
