using Microsoft.Extensions.Options;

namespace AurumPay.Checkout.Api.Infrastructure.Options;

public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";
    
    public void Configure(JwtOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}