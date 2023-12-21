using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Quizer.Domain.QuizAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Quizer.Infrastructure.Persistance
{
    public class QuizerDbContext : IdentityDbContext
    {
        public QuizerDbContext(DbContextOptions<QuizerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuizerDbContext).Assembly);

            modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties())
                .Where(p => p.IsPrimaryKey())
                .ToList()
                .ForEach(p => p.ValueGenerated = ValueGenerated.Never);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Quiz> Quizes { get; set; }
    }
}
