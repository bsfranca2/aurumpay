namespace AurumPay.Checkout.Presentation.CheckoutSessions;

public record IdentifyCustomerDto
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Cpf { get; set; }
}