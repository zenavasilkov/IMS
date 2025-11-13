using Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using RecruitmentNotifications.Messages;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal class ProcessOutboxMessagesJob(RecruitmentDbContext dbContext,
    IPublisher publisher, ILogger<ProcessOutboxMessagesJob> logger) : IJob
{
    private const int BatchSize = 20;

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(BatchSize)
            .ToListAsync(context.CancellationToken);

        foreach (var message in messages)
        {
            Type? eventType = null;

            var assembly = typeof(BaseEvent).Assembly;
                
            eventType = assembly.GetType(message.Type);

            if (eventType == null)
            {
                logger.LogWarning("Could not find type {TypeName} for outbox message with Id {MessageId}." +
                    " Assembly might not be loaded or name is incorrect.", message.Type, message.Id);

                continue;
            }

            var domainEvent = JsonConvert.DeserializeObject(message.Content, eventType);

            if (domainEvent is null)
            {
                logger.LogWarning("Failed to deserialize outbox message with Id {MessageId}", message.Id);

                continue;
            }

            await publisher.Publish(domainEvent, context.CancellationToken);

            message.ProcessedOnUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
