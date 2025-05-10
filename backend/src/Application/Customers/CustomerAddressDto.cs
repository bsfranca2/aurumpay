namespace AurumPay.Application.Customers;

public record CustomerAddressDto(
    long Id,
    string Cep,
    string AddressLine1,
    string AddressLine2,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string Recipient,
    bool IsMain
);