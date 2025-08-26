namespace AurumPay.Core;

public interface IEvent
{
    Guid EventId { get; }
    DateTime OccurredOnUtc { get; }
}