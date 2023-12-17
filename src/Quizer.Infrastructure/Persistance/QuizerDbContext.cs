using Microsoft.EntityFrameworkCore;
using Quizer.Domain.Entities;

namespace Quizer.Infrastructure.Persistance
{
    public class QuizerDbContext : DbContext
    {
        public QuizerDbContext(DbContextOptions<QuizerDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
