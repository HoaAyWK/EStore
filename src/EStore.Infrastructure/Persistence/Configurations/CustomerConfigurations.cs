using EStore.Domain.Common.ValueObjects;
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

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(Customer.PhoneNumberLength);

        builder.Property(u => u.Email)
            .IsUnicode()
            .HasMaxLength(255);

        builder.OwnsOne(u => u.Address, ab =>
        {
            ab.Property(a => a.Street)
                .HasMaxLength(Address.MaxStreetLength);

            ab.Property(a => a.City)
                .HasMaxLength(Address.MaxCityLength);

            ab.Property(a => a.State)
                .HasMaxLength(Address.MaxStateLength);

            ab.Property(a => a.Country)
                .HasMaxLength(Address.MaxCountryLength);
        });

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasIndex(u => u.PhoneNumber);

        builder.HasIndex(u => u.Id);

        builder.Ignore(u => u.FullName);

        builder.HasQueryFilter(u => !u.Deleted);
    }
}
