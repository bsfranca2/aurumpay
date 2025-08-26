using AurumPay.Domain.Outbox;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class OutboxRepository(DatabaseContext dbContext) : IOutboxRepository
{
    public async Task AddAsync(OutboxMessage message)
    {
        await dbContext.OutboxMessages.AddAsync(message);
        await dbContext.SaveChangesAsync();
    }
}