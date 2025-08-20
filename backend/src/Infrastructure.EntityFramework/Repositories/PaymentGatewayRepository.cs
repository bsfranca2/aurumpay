using AurumPay.Domain.Payments;
using AurumPay.Domain.Payments.Gateways;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class PaymentGatewayRepository(DatabaseContext dbContext)
    : Repository<PaymentGateway, PaymentGatewayId, DatabaseContext>(dbContext), IPaymentGatewayRepository
{
}