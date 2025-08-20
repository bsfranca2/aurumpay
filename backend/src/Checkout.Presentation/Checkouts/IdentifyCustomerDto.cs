namespace AurumPay.Checkout.Presentation.Checkouts;

public record IdentifyCustomerDto
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Cpf { get; set; }
}