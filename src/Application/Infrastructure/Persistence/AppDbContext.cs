using AurumPay.Application.Domain.Catalog;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Application.Infrastructure.Persistence;

// dotnet ef migrations add InitialData -p .\src\Application\ -s .\src\Api\ --context AppDbContext
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; init; }
}
