namespace BillLedger.Infrastructure.Data.Interceptors;

public class DispatchDomainEventsInterceptor(IEventMediator eventMediator) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await DispatchEventsAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchEventsAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null)
        {
            return;
        }

        var entitiesWithEvents = context.ChangeTracker
            .Entries<IHasDomainEvents>()
            .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        if (entitiesWithEvents.Count == 0)
        {
            return;
        }

        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await eventMediator.PublishAsync(domainEvent, cancellationToken);
        }
    }
}
