using IMS.NotificationsCore.Messages;
using Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

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
            try
            {
                Type? eventType = null;

                var assembly = typeof(BaseEvent).Assembly;

                eventType = assembly.GetType(message.Type);

                if (eventType == null)
                {
                    message.Error = $"Type '{message.Type}' not found.";

                    logger.LogWarning("Outbox message type not found. Type={Type}, MessageId={MessageId}", message.Type, message.Id);

                    message.ProcessedOnUtc = DateTime.UtcNow;

                    continue;
                }

                var domainEvent = JsonConvert.DeserializeObject(message.Content, eventType);

                if (domainEvent is null)
                {
                    message.Error = $"Failed to deserialize content for type '{message.Type}'.";

                    logger.LogWarning("Outbox message deserialization failed. Type={Type}, MessageId={MessageId}", message.Type, message.Id);

                    message.ProcessedOnUtc = DateTime.UtcNow;

                    continue;
                }

                await publisher.Publish(domainEvent, context.CancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                message.Error = ex.ToString();

                logger.LogError(ex, "Outbox message processing failed. Type={Type}, MessageId={MessageId}", message.Type, message.Id);
            }            
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
