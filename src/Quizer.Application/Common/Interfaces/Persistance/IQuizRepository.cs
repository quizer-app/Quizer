using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Entities;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Application.Common.Interfaces.Persistance;

public interface IQuizRepository
{
    Task AddQuiz(Quiz quiz);
    Task<List<Quiz>> GetAllQuestions(Guid? userId = null);
    Task<Quiz?> GetQuiz(QuizId id);
    Task<Quiz?> GetQuiz(string userName, string quizName);
    Task<Question?> GetQuestion(QuestionId id);
    void DeleteQuiz(Quiz quiz);
    void DeleteQuestion(Question id);
}
