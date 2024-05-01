using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.NotificationAggregate;
using EStore.Domain.NotificationAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EStore.Infrastructure.Persistence.Configurations;

public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        ConfigureNotificationsTable(builder);
    }

    private void ConfigureNotificationsTable(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasColumnName("NotificationId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => NotificationId.Create(value));

        builder.Property(n => n.From)
            .HasColumnName("From")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CustomerId.Create(value));

        builder.Property(n => n.To)
            .HasColumnName("To")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CustomerId.Create(value));

        builder.Property(n => n.Domain)
            .HasMaxLength(50);

        builder.Property(n => n.Type)
            .HasMaxLength(50);
    }
}
