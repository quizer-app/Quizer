using Microsoft.EntityFrameworkCore;
using Quizer.Domain.QuizAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Quizer.Domain.UserAggregate;
using Quizer.Infrastructure.Persistance.Interceptors;
using Quizer.Domain.Common.Models;
using Quizer.Domain.QuestionAggregate;
using Quizer.Domain.RefreshTokenAggregate;

namespace Quizer.Infrastructure.Persistance;

public class QuizerDbContext : IdentityDbContext<User>
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public QuizerDbContext(DbContextOptions<QuizerDbContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(QuizerDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    public override DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Quiz> Quizes { get; set; }
    public DbSet<Question> Questions { get; set; }
}
