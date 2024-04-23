using EStore.Domain.CustomerAggregate;
using EStore.Domain.CustomerAggregate.Entities;
using EStore.Domain.CustomerAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        ConfigureCustomersTable(builder);
        ConfigureAddressesTable(builder);
    }

    private void ConfigureCustomersTable(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

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
            .HasMaxLength(Customer.MaxPhoneNumberLength);

        builder.Property(u => u.Email)
            .IsUnicode()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasIndex(u => u.PhoneNumber);

        builder.HasIndex(u => u.Id);

        builder.Ignore(u => u.FullName);

        builder.HasQueryFilter(u => !u.Deleted);
    }

    private void ConfigureAddressesTable(EntityTypeBuilder<Customer> builder)
    {
        builder.OwnsMany(c => c.Addresses, ab =>
        {
            ab.ToTable("CustomerAddresses");

            ab.WithOwner().HasForeignKey("CustomerId");

            ab.HasKey(nameof(Address.Id), "CustomerId");

            ab.Property(a => a.Id)
                .HasColumnName("AddressId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => AddressId.Create(value));

            ab.Property(a => a.ReceiverName)
                .HasMaxLength(Address.MaxReceiverNameLength);

            ab.Property(a => a.PhoneNumber)
                .HasMaxLength(Address.MaxPhoneNumberLength);

            ab.Property(a => a.Street)
                .HasMaxLength(Address.MaxStreetLength);

            ab.Property(a => a.City)
                .HasMaxLength(Address.MaxCityLength);

            ab.Property(a => a.State)
                .HasMaxLength(Address.MaxStateLength);

            ab.Property(a => a.Country)
                .HasMaxLength(Address.MaxCountryLength);

            ab.Property(a => a.ZipCode)
                .HasMaxLength(Address.MaxZipCodeLength);
        });

        builder.Metadata.FindNavigation(nameof(Customer.Addresses))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
