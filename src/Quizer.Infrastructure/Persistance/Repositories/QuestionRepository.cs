using Microsoft.EntityFrameworkCore;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Infrastructure.Persistance.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly QuizerDbContext _context;

    public QuestionRepository(QuizerDbContext context)
    {
        _context = context;
    }

    public async Task Add(Question question)
    {
        await _context.Questions.AddAsync(question);
    }

    public void Delete(Question question)
    {
        _context.Questions.Remove(question);
    }

    public async Task<Question?> Get(QuestionId id)
    {
        return await _context.Questions.FirstOrDefaultAsync(qs => qs.Id == id);
    }

    public async Task<List<Question>> GetAllQuestions()
    {
        return await _context.Questions.ToListAsync();
    }
}