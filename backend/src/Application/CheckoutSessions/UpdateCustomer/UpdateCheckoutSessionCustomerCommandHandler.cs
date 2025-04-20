using Ardalis.Result;

using AurumPay.Application.SeedWork;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Shared;

namespace AurumPay.Application.CheckoutSessions.UpdateCustomer;

public class UpdateCheckoutSessionCustomerCommandHandler(
    ICheckoutContext checkoutContext,
    IUnitOfWork unitOfWork,
    ICustomerRepository customerRepository,
    IStoreCustomerService storeCustomerService,
    ICheckoutSessionRepository checkoutSessionRepository)
    : ICommandHandler<UpdateCheckoutSessionCustomerCommand, Result>
{
    public async Task<Result> Handle(UpdateCheckoutSessionCustomerCommand request, CancellationToken cancellationToken)
    {
        CheckoutSession? checkoutSession = await checkoutContext.SessionManager.GetCurrentSessionAsync();

        if (checkoutSession is null)
        {
            return Result.Invalid(new ValidationError("No active checkout session found"));
        }

        await unitOfWork.BeginTransactionAsync();

        Customer? customer =
            await storeCustomerService.FindByEmailAsync(new EmailAddress(request.Email), cancellationToken);

        if (customer == null)
        {
            Customer newCustomer = Customer.Create(checkoutContext.Store.GetCurrentStoreId(), request.FullName,
                new EmailAddress(request.Email), new Cpf(request.Cpf), new Telephone(request.PhoneNumber));
            customer = await customerRepository.CreateAsync(newCustomer);
        }

        checkoutSession.IdentifyCustomer(customer);
        await checkoutSessionRepository.UpdateAsync(checkoutSession);

        await unitOfWork.CommitAsync();

        return Result.Success();
    }
}