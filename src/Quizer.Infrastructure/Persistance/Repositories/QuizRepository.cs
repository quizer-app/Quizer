using Microsoft.EntityFrameworkCore;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Infrastructure.Persistance.Repositories;


public class QuizRepository : IQuizRepository
{
    private readonly QuizerDbContext _context;
    public IQueryable<Quiz> Quizes => _context.Quizes;

    public QuizRepository(QuizerDbContext context)
    {
        _context = context;
    }

    public async Task Add(Quiz quiz)
    {
        await _context.Quizes.AddAsync(quiz);
    }

    public void Delete(Quiz quiz)
    {
        _context.Quizes.Remove(quiz);
    }

    public async Task<Quiz?> Get(QuizId id)
    {
        return await _context.Quizes.FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<Quiz?> Get(string userName, string quizSlug)
    {
        return await _context.Quizes.FirstOrDefaultAsync(q => q.UserName == userName && q.Slug == quizSlug);
    }
}