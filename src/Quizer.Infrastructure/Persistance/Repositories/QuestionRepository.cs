using Microsoft.EntityFrameworkCore;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.QuizAggregate;

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

    public async Task<Question?> Get(QuestionId questionId)
    {
        return await _context.Questions.FirstOrDefaultAsync(qs => qs.Id == questionId);
    }

    public async Task<List<Question>> GetAllQuestions(QuizId quizId)
    {
        return await _context.Questions
            .Where(qs => qs.QuizId == quizId)
            .OrderBy(qs => qs.CreatedAt)
            .ToListAsync();
    }
}