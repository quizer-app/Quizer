using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Common.Interfaces.Persistance;

public interface IQuizRepository
{
    IQueryable<Quiz> Quizes { get; }
    
    Task Add(Quiz quiz);
    Task<Quiz?> Get(QuizId id);
    Task<Quiz?> Get(string userName, string quizSlug);
    void Delete(Quiz quiz);
}
