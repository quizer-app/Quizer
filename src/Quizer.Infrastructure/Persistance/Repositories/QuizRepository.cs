using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Infrastructure.Persistance.Repositories
{
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
            await _context.SaveChangesAsync();
        }
    }
}
