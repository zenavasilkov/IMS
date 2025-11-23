using IMS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IMS.DAL.Interceptors;

public class UpdateTimestampsInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = context.ChangeTracker.Entries<EntityBase>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        var utcNow = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            entry.Entity.ModifiedAt = utcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
