using AurumPay.Domain.Catalog;
using AurumPay.Domain.Merchants;
using AurumPay.Domain.Payments.Configuration;
using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.Shared;
using AurumPay.Domain.Stores;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.ValueGenerators;

public static class DatabaseContextSeed
{
    public static async Task Seed(DatabaseContext context, CancellationToken cancellationToken = default)
    {
        await SeedPayments(context, cancellationToken);
        await SeedStoresAndProducts(context, cancellationToken);
        await SeedStoreConfigurations(context, cancellationToken);
    }

    private static async Task SeedPayments(DatabaseContext context, CancellationToken cancellationToken = default)
    {
        if (await context.PaymentMethods.AnyAsync(cancellationToken))
        {
            return;
        }

        PaymentMethod creditCardMethod = PaymentMethod.Create("Credit Card", PaymentMethodType.CreditCard);
        await context.AddAsync(creditCardMethod, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        PaymentGateway mercadoPagoGateway = PaymentGateway.Create("mercadopago", "Mercado Pago", PaymentGatewayType.MercadoPago);
        await context.AddAsync(mercadoPagoGateway, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedStoresAndProducts(DatabaseContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Merchants.AnyAsync(cancellationToken))
        {
            return;
        }

        Merchant merchant = new(new MerchantId(), "João Ap", new EmailAddress("joao@email.com"));
        await context.AddAsync(merchant, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        Store clothesStore = new(new StoreId(), merchant.Id, "Clothes Store");
        await context.AddAsync(clothesStore, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        Product clothesProduct1 = Product.CreateNew(clothesStore.Id, "Camiseta Básica", 29.99m);
        Product clothesProduct2 = Product.CreateNew(clothesStore.Id, "Calça Jeans", 89.90m);
        Product clothesProduct3 = Product.CreateNew(clothesStore.Id, "Jaqueta de Couro", 249.99m);

        await context.AddAsync(clothesProduct1, cancellationToken);
        await context.AddAsync(clothesProduct2, cancellationToken);
        await context.AddAsync(clothesProduct3, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        Store shoesStore = new(new StoreId(), merchant.Id, "Shoes Store");
        await context.AddAsync(shoesStore, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        Product shoesProduct1 = Product.CreateNew(shoesStore.Id, "Tênis Esportivo", 149.90m);
        Product shoesProduct2 = Product.CreateNew(shoesStore.Id, "Sapato Social", 179.99m);
        Product shoesProduct3 = Product.CreateNew(shoesStore.Id, "Sandália de Verão", 59.90m);

        await context.AddAsync(shoesProduct1, cancellationToken);
        await context.AddAsync(shoesProduct2, cancellationToken);
        await context.AddAsync(shoesProduct3, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedStoreConfigurations(DatabaseContext context, CancellationToken cancellationToken = default)
    {
        if (await context.StorePaymentConfigurations.AnyAsync(cancellationToken))
        {
            return;
        }

        List<StoreId> stores = await context.Stores
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);

        PaymentMethod creditCardMethod = await context.PaymentMethods
            .FirstAsync(pt => pt.Type == PaymentMethodType.CreditCard, cancellationToken);

        PaymentGateway mercadoPagoGateway = await context.PaymentGateways
            .FirstAsync(pg => pg.Type == PaymentGatewayType.MercadoPago, cancellationToken);

        foreach (StoreId storeId in stores)
        {
            PaymentGatewayCredentials credentials = new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            
            StorePaymentConfiguration storePaymentConfiguration = StorePaymentConfiguration
                .Create(storeId, creditCardMethod.Id, mercadoPagoGateway.Id, credentials);
            
            await context.AddAsync(storePaymentConfiguration, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}