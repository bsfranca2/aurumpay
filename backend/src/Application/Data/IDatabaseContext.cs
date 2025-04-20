using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Application.Data;

public interface IDatabaseContext
{
    DbSet<CheckoutSession> CheckoutSessions { get; }
    DbSet<Customer> Customers { get; }
}