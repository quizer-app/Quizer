using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Common.Interfaces.Persistance;

public interface IQuestionRepository
{
    Task Add(Question question);
    void Delete(Question question);
    Task<Question?> Get(QuestionId id);
    Task<List<Question>> GetAllQuestions();
}