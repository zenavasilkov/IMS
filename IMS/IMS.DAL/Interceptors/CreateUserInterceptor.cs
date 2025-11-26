using IMS.DAL.Entities;
using IMS.DAL.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace IMS.DAL.Interceptors;

public class CreateUserInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null) return ValueTask.FromResult(result);

        var entries = context.ChangeTracker.Entries<User>()
            .Where(e => e.State == EntityState.Added).ToList();

        foreach (var entry in entries)
        {
            var outboxMessage = BuildOutboxMessage(entry.Entity);

            context.Set<OutboxMessage>().Add(outboxMessage);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static OutboxMessage BuildOutboxMessage(User user)
    {
        var createAuth0User = new CreateAuth0User(user.Email, user.Role.ToString());
        
        var content = JsonConvert.SerializeObject(createAuth0User, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });

        var outboxMessage = new OutboxMessage()
        {
            Type = user.GetType().FullName!,
            Content = content
        };

        return outboxMessage;
    }
}
