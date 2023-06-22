using EStore.Domain.CustomerAggregate;
using EStore.Domain.CustomerAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("CustomerId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CustomerId.Create(value));

        builder.Property(u => u.FirstName)
            .HasMaxLength(Customer.MaxFirstNameLength);

        builder.Property(u => u.LastName)
            .HasMaxLength(Customer.MaxLastNameLength);

        builder.Property(u => u.Email)
            .IsUnicode()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasIndex(u => u.Id);

        builder.Ignore(u => u.FullName);
    }
}
