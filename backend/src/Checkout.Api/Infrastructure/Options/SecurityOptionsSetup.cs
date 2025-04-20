using Microsoft.Extensions.Options;

namespace AurumPay.Checkout.Api.Infrastructure.Options;

public class SecurityOptionsSetup(IConfiguration configuration) : IConfigureOptions<SecurityOptions>
{
    private const string ConfigurationSchemaName = "Security";

    public void Configure(SecurityOptions options)
    {
        configuration.GetSection(ConfigurationSchemaName).Bind(options);
    }
}