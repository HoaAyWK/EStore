using EStore.Application.Common.Interfaces.Services;

namespace EStore.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
