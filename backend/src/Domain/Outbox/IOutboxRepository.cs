namespace AurumPay.Domain.Outbox;

public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage message);
}