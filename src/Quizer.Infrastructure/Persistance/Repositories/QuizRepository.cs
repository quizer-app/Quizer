using Microsoft.EntityFrameworkCore;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Infrastructure.Persistance.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly QuizerDbContext _context;

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

    public async Task<Quiz?> Get(string name)
    {
        return await _context.Quizes.FirstOrDefaultAsync(q => q.Name == name);
    }

    public async Task<List<Quiz>> GetAll(Guid? userId = null)
    {
        if(userId is null)
            return await _context.Quizes.ToListAsync();
        else
            return await _context.Quizes.Where(q => q.UserId == userId).ToListAsync();
    }
}
