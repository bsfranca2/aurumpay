using FluentValidation;

namespace AurumPay.Application.CheckoutSessions.Create;

public class CreateCheckoutSessionCommandValidator : AbstractValidator<CreateCheckoutSessionCommand>
{
    private const int MaxQuantity = 1_000_000;
    
    public CreateCheckoutSessionCommandValidator()
    {
        RuleFor(command => command.CartItems)
            .NotNull()
            .WithMessage("The 'CartItems' dictionary cannot be null.");

        RuleForEach(command => command.CartItems).ChildRules(pair =>
        {
            pair.RuleFor(kvp => kvp.Key)
                .NotEmpty()
                .WithMessage("The key (Product ID) in 'CartItems' cannot be empty.")
                .Length(10)
                .WithMessage("The key (Product ID) in 'CartItems' must be exactly 10 characters long.");

            pair.RuleFor(kvp => kvp.Value)
                .GreaterThan(0)
                .WithMessage("The quantity (value) for each product must be greater than zero.")
                .LessThanOrEqualTo(MaxQuantity)
                .WithMessage($"The quantity (value) for each product cannot exceed {MaxQuantity:N0}.");
        });
    }
}