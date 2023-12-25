using Quizer.Application.Common.Interfaces.Persistance;

namespace Quizer.Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuizerDbContext _context;

        public UnitOfWork(QuizerDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync();
        }
    }
}
