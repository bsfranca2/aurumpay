using AurumPay.Domain.CheckoutSessions;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class CheckoutSessionRepository(DatabaseContext context)
    : Repository<CheckoutSession, CheckoutSessionId, DatabaseContext>(context), ICheckoutSessionRepository
{
}