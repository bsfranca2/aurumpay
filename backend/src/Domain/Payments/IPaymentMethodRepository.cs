using AurumPay.Core;
using AurumPay.Domain.Payments.Methods;

namespace AurumPay.Domain.Payments;

public interface IPaymentMethodRepository : IRepository<PaymentMethod, PaymentMethodId>
{
    Task<PaymentMethod?> GetActiveByIdAsync(PaymentMethodId paymentMethodId);
    Task<PaymentMethod?> GetActiveByPaymentMethodTypeAsync(PaymentMethodType paymentMethodType);
}