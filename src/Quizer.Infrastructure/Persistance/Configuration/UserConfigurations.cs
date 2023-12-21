using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizer.Domain.UserAggregate;

namespace Quizer.Infrastructure.Persistance.Configuration
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            ConfigureUsersTable(builder);
        }

        private static void ConfigureUsersTable(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value)
                    );
        }
    }
}
