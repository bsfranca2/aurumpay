using AurumPay.Application.Customers;

namespace AurumPay.Application.CheckoutSessions;

public record CustomerDto(
    string FullName,
    string Email,
    string Cpf,
    string PhoneNumber,
    IEnumerable<CustomerAddressDto> Addresses,
    bool IsProspect
);
