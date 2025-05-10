using System.Text.RegularExpressions;

using AurumPay.Application.Customers;

using FluentValidation;

namespace AurumPay.Application.CheckoutSessions.UpdateCustomer;

public class UpdateCheckoutSessionCustomerCommandValidator : AbstractValidator<UpdateCheckoutSessionCustomerCommand>
{
    public UpdateCheckoutSessionCustomerCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Campo obrigatório")
            .Must(FullNameValidator.Validate).WithMessage("Digite o seu nome completo");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Campo obrigatório")
            .EmailAddress().WithMessage("E-mail inválido. Verifique se digitou corretamente");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("Campo obrigatório")
            .Must(BeValidCpf).WithMessage("Digite um CPF válido");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Campo obrigatório")
            .Must(BeValidPhoneNumber).WithMessage("Telefone inválido");
    }

    private bool BeValidCpf(string cpf)
    {
        string numericCpf = Regex.Replace(cpf, @"[^\d]", "");

        if (numericCpf.Length != 11)
        {
            return false;
        }

        bool allDigitsEqual = true;
        for (int i = 1; i < 11; i++)
        {
            if (numericCpf[i] != numericCpf[0])
            {
                allDigitsEqual = false;
                break;
            }
        }

        if (allDigitsEqual)
        {
            return false;
        }

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(numericCpf[i].ToString()) * (10 - i);
        }

        int firstVerifier = sum % 11 < 2 ? 0 : 11 - (sum % 11);
        if (firstVerifier != int.Parse(numericCpf[9].ToString()))
        {
            return false;
        }

        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(numericCpf[i].ToString()) * (11 - i);
        }

        int secondVerifier = sum % 11 < 2 ? 0 : 11 - (sum % 11);
        return secondVerifier == int.Parse(numericCpf[10].ToString());
    }

    private bool BeValidPhoneNumber(string phoneNumber)
    {
        string numericPhone = Regex.Replace(phoneNumber, @"[^\d]", "");

        return numericPhone.Length == 10 || numericPhone.Length == 11;
    }
}