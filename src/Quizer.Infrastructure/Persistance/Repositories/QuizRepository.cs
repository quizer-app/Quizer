using Microsoft.EntityFrameworkCore;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Entities;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Infrastructure.Persistance.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly QuizerDbContext _context;

    public QuizRepository(QuizerDbContext context)
    {
        _context = context;
    }

    public async Task AddQuiz(Quiz quiz)
    {
        await _context.Quizes.AddAsync(quiz);
    }

    public void DeleteQuiz(Quiz quiz)
    {
        _context.Quizes.Remove(quiz);
    }

    public void DeleteQuestion(Question question)
    {
        _context.Questions.Remove(question);
    }

    public async Task<Quiz?> GetQuiz(QuizId id)
    {
        return await _context.Quizes.FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<Question?> GetQuestion(QuestionId id)
    {
        return await _context.Questions.FirstOrDefaultAsync(qs => qs.Id == id);
    }

    public async Task<Quiz?> GetQuiz(string userName, string quizName)
    {
        return await _context.Quizes.FirstOrDefaultAsync(q => q.UserName == userName && q.Name == quizName);
    }

    public async Task<List<Quiz>> GetAllQuestions(Guid? userId = null)
    {
        if(userId is null)
            return await _context.Quizes.ToListAsync();
        else
            return await _context.Quizes.Where(q => q.UserId == userId).ToListAsync();
    }
}
