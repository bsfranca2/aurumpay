using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.SeedWork;

namespace AurumPay.Domain.Payments;

public interface IPaymentGatewayRepository : IRepository<PaymentGateway, PaymentGatewayId>
{
}