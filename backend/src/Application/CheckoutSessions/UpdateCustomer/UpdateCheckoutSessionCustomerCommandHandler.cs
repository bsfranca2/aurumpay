using Ardalis.Result;

using AurumPay.Application.Interfaces;
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
    ICheckoutSessionRepository checkoutSessionRepository
) : ICommandHandler<UpdateCheckoutSessionCustomerCommand, Result>
{
    public async Task<Result> Handle(UpdateCheckoutSessionCustomerCommand request, CancellationToken cancellationToken)
    {
        CheckoutSession? checkoutSession = await checkoutContext.SessionManager.GetCurrentSessionAsync();

        if (checkoutSession is null)
        {
            return Result.Error("No active checkout session found");
        }

        await unitOfWork.BeginTransactionAsync();

        Customer? customer = await customerRepository.FindByEmailAsync(
            checkoutSession.StoreId,
            new EmailAddress(request.Email),
            cancellationToken);

        if (customer == null)
        {
            Customer newCustomer = Customer.CreateProspect(checkoutContext.Store.GetCurrentStoreId(), request.FullName,
                new EmailAddress(request.Email), new Cpf(request.Cpf), new Telephone(request.PhoneNumber));
            customer = await customerRepository.CreateAsync(newCustomer);
        }

        checkoutSession.IdentifyCustomer(customer);
        await checkoutSessionRepository.UpdateAsync(checkoutSession);

        await unitOfWork.CommitTransactionAsync();

        return Result.Success();
    }
}