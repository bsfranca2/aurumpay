using AurumPay.Domain.Customers;
using AurumPay.Domain.Stores;
using AurumPay.Infrastructure.EntityFramework.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> customerConfiguration)
    {
        customerConfiguration.ToTable("Customers");

        customerConfiguration.HasKey(c => c.Id);
        
        customerConfiguration
            .Property(c => c.Id)
            .HasConversion(new CustomerIdConverter())
            .ValueGeneratedOnAdd()
            .HasIdentityOptions(startValue: 1000, incrementBy: 1)
            .HasColumnType("bigint");

        customerConfiguration
            .Property(c => c.StoreId)
            .HasConversion(new StoreIdConverter());
        
        customerConfiguration
            .HasOne<Store>()
            .WithMany()
            .HasForeignKey(s => s.StoreId);

        // TODO: customerConfiguration.HasIndex(c => c.StoreId); Index for admin

        customerConfiguration
            .Property(c => c.FullName)
            .HasMaxLength(255);

        customerConfiguration
            .Property(c => c.Email)
            .HasConversion(new EmailAddressConverter())
            .HasMaxLength(255);

        customerConfiguration
            .Property(c => c.Cpf)
            .HasConversion(new CpfConverter())
            .HasMaxLength(11);

        customerConfiguration
            .Property(c => c.MobilePhone)
            .HasConversion(new TelephoneConverter())
            .HasMaxLength(15);

        customerConfiguration
            .HasIndex(c => new { c.StoreId, c.Email })
            .IsUnique();
        
        customerConfiguration
            .HasMany(c => c.Addresses)
            .WithOne()
            .HasForeignKey("CustomerId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}