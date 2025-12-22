using IMS.DAL.Entities;
using IMS.DAL.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using Shared.Enums;

namespace IMS.DAL.Interceptors;

public class UpdateUserRoleInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null) return ValueTask.FromResult(result);

        var entries = context.ChangeTracker.Entries<User>()
            .Where(e => e.State == EntityState.Modified).ToList();

        foreach (var entry in entries)
        {
            var roleEntry = entry.Property(nameof(User.Role));

            if (roleEntry?.IsModified != true) continue;
            var newRole = (Role)roleEntry.CurrentValue!;
            
            var outboxMessage = BuildOutboxMessage(entry.Entity, newRole);
            context.Set<OutboxMessage>().Add(outboxMessage);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static OutboxMessage BuildOutboxMessage(User user, Role newRole)
    {
        var updatePayload = new OutboxUserRoleUpdate(
            user.Id, 
            user.Email,
            newRole.ToString()
        );

        var content = JsonConvert.SerializeObject(updatePayload, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });

        var outboxMessage = new OutboxMessage()
        {
            Type = "UserRoleUpdate",
            Content = content
        };

        return outboxMessage;
    }
}
