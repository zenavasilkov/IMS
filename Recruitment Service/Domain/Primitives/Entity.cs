using IMS.NotificationsCore.Messages;

namespace Domain.Primitives;

public abstract class Entity(Guid id) : IEquatable<Entity>
{
    public Guid Id { get; private init; } = id;

    private readonly List<BaseEvent> _domainEvents = [];

    public List<BaseEvent> GetDomainEvents() => [.._domainEvents]; 

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void Raise(BaseEvent domainEvent) => _domainEvents.Add(domainEvent);

    public static bool operator ==(Entity? first, Entity? second) => 
        first is not null && second is not null && first.Equals(second);

    public static bool operator !=(Entity? first, Entity? second) => !(first == second);

    public override bool Equals(object? obj)
    {
        if(obj is null) return false;

        if(obj.GetType() != GetType()) return false;

        if (obj is not Entity other) return false; 

        return other.Id == Id;
    }

    public bool Equals(Entity? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other)) return true;

        return other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
