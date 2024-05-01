namespace EStore.Infrastructure.BackgroundJobs.Options;

public class ProcessOutboxMessagesJobOptions
{
    public const string SectionName = "ProcessOutboxMessagesJob";

    public int IntervalInSeconds { get; set; }
}
