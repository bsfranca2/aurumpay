using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.SeedWork;

namespace AurumPay.Domain.Payments;

public interface IPaymentMethodRepository : IRepository<PaymentMethod, PaymentMethodId>
{
    Task<PaymentMethod?> GetActiveByPaymentMethodTypeAsync(PaymentMethodType paymentMethodType);
}