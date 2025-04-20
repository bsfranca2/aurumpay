namespace AurumPay.Application.Customers;

public record CustomerAddressDto(
    long Id,
    string Cep,
    string AddressLine1,
    string AddressLine2,
    string Number,
    string City,
    string State,
    bool isMain
);