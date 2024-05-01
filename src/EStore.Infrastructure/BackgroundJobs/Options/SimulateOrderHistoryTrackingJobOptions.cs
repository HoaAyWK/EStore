namespace EStore.Infrastructure.BackgroundJobs.Options;

public class SimulateOrderHistoryTrackingJobOptions
{
    public const string SectionName = "SimulateOrderHistoryTrackingJob";

    public int IntervalInSeconds { get; set; }
}
