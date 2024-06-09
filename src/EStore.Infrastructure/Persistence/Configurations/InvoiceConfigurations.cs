using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.InvoiceAggregate;
using EStore.Domain.InvoiceAggregate.ValueObjects;
using EStore.Domain.OrderAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public sealed class InvoiceConfigurations : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(invoice => invoice.Id);

        builder.Property(invoice => invoice.Id)
            .HasColumnName("InvoiceId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => InvoiceId.Create(value));

        builder.Property(invoice => invoice.InvoiceBlobUrl)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(invoice => invoice.OrderId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value));

        builder.Property(invoice => invoice.CustomerId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CustomerId.Create(value));

        builder.HasQueryFilter(invoice => !invoice.Deleted);
    }
}