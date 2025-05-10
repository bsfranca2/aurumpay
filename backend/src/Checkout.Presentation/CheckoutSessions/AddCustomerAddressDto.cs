namespace AurumPay.Checkout.Presentation.CheckoutSessions;

public record AddCustomerAddressDto
{
    public required string Cep { get; set; }
    public required string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; } = string.Empty;
    public required string Number { get; set; }
    public required string Neighborhood { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Recipient { get; set; }
}