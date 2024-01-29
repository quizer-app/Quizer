using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizer.Domain.RefreshTokenAggregate;

namespace Quizer.Infrastructure.Persistance.Configuration;

public class RefreshTokenConfigurations : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        ConfigureRefreshTokenTable(builder);
    }

    private static void ConfigureRefreshTokenTable(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => TokenId.Create(value)
                );
    }
}
