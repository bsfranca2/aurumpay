namespace AurumPay.Checkout.Api.Infrastructure.Options;

public class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int TokenLifetimeMinutes { get; set; }
}