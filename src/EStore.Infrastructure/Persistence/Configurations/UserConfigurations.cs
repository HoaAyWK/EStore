using EStore.Domain.UserAggregate;
using EStore.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("UserId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.Property(u => u.FirstName)
            .HasMaxLength(User.MaxFirstNameLength);

        builder.Property(u => u.LastName)
            .HasMaxLength(User.MaxLastNameLength);

        builder.Property(u => u.Email)
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email);

        builder.HasIndex(u => u.Id);

        builder.Ignore(u => u.FullName);
    }
}
