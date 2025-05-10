using FluentValidation;

namespace AurumPay.Application.Customers.AddAddress;

public class AddCustomerAddressCommandValidator : AbstractValidator<AddCustomerAddressCommand>
{
    public AddCustomerAddressCommandValidator()
    {
        RuleFor(x => x.Cep)
            .NotEmpty().WithMessage("CEP inválido")
            .Length(8).WithMessage("CEP inválido");

        RuleFor(x => x.AddressLine1)
            .NotEmpty().WithMessage("Campo obrigatório")
            .MinimumLength(5).WithMessage("Campo inválido");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Campo obrigatório")
            .MinimumLength(1).WithMessage("Campo inválido")
            .MaximumLength(6).WithMessage("Campo inválido");

        RuleFor(x => x.Neighborhood)
            .NotEmpty().WithMessage("Campo obrigatório")
            .MinimumLength(3).WithMessage("Campo inválido")
            .MaximumLength(40).WithMessage("Campo inválido");

        When(x => !string.IsNullOrEmpty(x.AddressLine2), () =>
        {
            RuleFor(x => x.AddressLine2)
                .MaximumLength(30).WithMessage("Campo inválido");
        });

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Campo obrigatório");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("Campo obrigatório")
            .Length(2).WithMessage("Campo inválido");

        RuleFor(x => x.Recipient)
            .NotEmpty().WithMessage("Campo obrigatório")
            .Must(recipient =>
            {
                string? trimmedRecipient = recipient?.Trim();
                return trimmedRecipient?.Length >= 3;
            }).WithMessage("Campo inválido")
            .Must(FullNameValidator.Validate).WithMessage("Campo inválido");
    }
}