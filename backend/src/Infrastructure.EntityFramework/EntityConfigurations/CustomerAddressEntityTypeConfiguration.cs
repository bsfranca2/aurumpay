using AurumPay.Domain.Customers;
using AurumPay.Infrastructure.EntityFramework.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AurumPay.Infrastructure.EntityFramework.EntityConfigurations;

public class CustomerAddressEntityTypeConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> customerAddressConfiguration)
    {
        customerAddressConfiguration.ToTable("CustomerAddresses");

        customerAddressConfiguration.HasKey(ca => ca.Id);

        customerAddressConfiguration
            .Property(ca => ca.Id)
            .HasConversion(new CustomerAddressIdConverter())
            .ValueGeneratedOnAdd()
            .HasIdentityOptions(1000, 1)
            .HasColumnType("bigint");
        
        customerAddressConfiguration
            .Property<CustomerId>("CustomerId")
            .HasConversion(new CustomerIdConverter())
            .HasColumnType("bigint");
        
        customerAddressConfiguration
            .HasIndex("CustomerId");

        customerAddressConfiguration
            .Property(ca => ca.Cep)
            .HasConversion(new CepConverter())
            .HasMaxLength(8);
        
        customerAddressConfiguration
            .Property(ca => ca.AddressLine1)
            .HasMaxLength(255);

        customerAddressConfiguration
            .Property(ca => ca.AddressLine2)
            .HasMaxLength(255);

        customerAddressConfiguration
            .Property(ca => ca.Number)
            .HasMaxLength(50);

        // TODO: Check city database
        customerAddressConfiguration
            .Property(ca => ca.City)
            .HasMaxLength(255);

        // TODO: Check state database
        customerAddressConfiguration
            .Property(ca => ca.State)
            .HasMaxLength(255);

        customerAddressConfiguration.Property(ca => ca.IsMain);
    }
}