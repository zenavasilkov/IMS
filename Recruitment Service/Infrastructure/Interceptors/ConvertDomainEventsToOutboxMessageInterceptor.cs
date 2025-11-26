using Domain.Primitives;
using IMS.NotificationsCore.Messages;
using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Infrastructure.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessageInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null) 
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var events = context.ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(e => {
                var domainEvents = e.GetDomainEvents();
                e.ClearDomainEvents();
                return domainEvents;
            })
            .Select(ConvertToOutboxMessage)
            .ToList();

        context.Set<OutboxMessage>().AddRange(events);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static OutboxMessage ConvertToOutboxMessage(BaseEvent domainEvent)
    {
        var content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });

        return new OutboxMessage
        {
            Type = domainEvent.GetType().FullName!,
            Content = content,
            OccurredOnUtc = DateTime.UtcNow
        };
    }
}
