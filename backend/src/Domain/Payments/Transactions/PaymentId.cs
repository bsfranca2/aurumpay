namespace AurumPay.Domain.Payments.Transactions;

public readonly record struct PaymentId(Ulid Value)
{
    public PaymentId() : this(Ulid.NewUlid()) { }
}