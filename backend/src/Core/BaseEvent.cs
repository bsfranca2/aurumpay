namespace AurumPay.Core;

public abstract record BaseEvent : IEvent
{
    public Guid EventId { get; } = Ulid.NewUlid().ToGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}