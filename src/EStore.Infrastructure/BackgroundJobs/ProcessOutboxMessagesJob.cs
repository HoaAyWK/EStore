using EStore.Domain.Common.Models;
using EStore.Infrastructure.Persistence;
using EStore.Infrastructure.Persistence.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Quartz;

namespace EStore.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly EStoreDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(EStoreDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (OutboxMessage outboxMessage in messages)
        {
            var domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                continue;
            }

            var pipeline = new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions()
                {
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Constant,
                    Delay = TimeSpan.Zero,
                    ShouldHandle = new PredicateBuilder()
                        .Handle<Exception>()
                })
                .Build();

            await pipeline.ExecuteAsync(async (token) =>
                await _publisher.Publish(domainEvent, token));
            
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
    }
}
