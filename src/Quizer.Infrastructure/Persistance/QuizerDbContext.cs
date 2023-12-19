using Microsoft.EntityFrameworkCore;
using Quizer.Domain.Entities;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Infrastructure.Persistance
{
    public class QuizerDbContext : DbContext
    {
        public QuizerDbContext(DbContextOptions<QuizerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuizerDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizes { get; set; }
    }
}
