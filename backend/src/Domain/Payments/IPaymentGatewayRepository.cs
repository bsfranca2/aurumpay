using AurumPay.Core;
using AurumPay.Domain.Payments.Gateways;

namespace AurumPay.Domain.Payments;

public interface IPaymentGatewayRepository : IRepository<PaymentGateway, PaymentGatewayId>
{
}