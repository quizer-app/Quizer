using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Common.Interfaces.Persistance;

public interface IQuestionRepository
{
    Task Add(Question question);
    void Delete(Question question);
    Task<Question?> Get(QuestionId questionId);
    Task<List<Question>> GetAllQuestions(QuizId quizId);
}