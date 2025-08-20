using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Methods;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class PaymentMethodRepository(DatabaseContext dbContext)
    : Repository<PaymentMethod, PaymentMethodId, DatabaseContext>(dbContext), IPaymentMethodRepository
{
    public async Task<PaymentMethod?> GetActiveByPaymentMethodTypeAsync(PaymentMethodType paymentMethodType)
    {
        return await DbSet
            .FirstOrDefaultAsync(pm => pm.Type == paymentMethodType && pm.IsActive);
    }
}