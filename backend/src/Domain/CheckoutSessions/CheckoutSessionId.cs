namespace AurumPay.Domain.CheckoutSessions;

public readonly record struct CheckoutSessionId(Ulid Value)
{
    public CheckoutSessionId() : this(Ulid.NewUlid()) { }
}