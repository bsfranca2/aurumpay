using System.IdentityModel.Tokens.Jwt;

using AurumPay.Checkout.Api;
using AurumPay.Checkout.Api.Infrastructure.Endpoints;
using AurumPay.Infrastructure.EntityFramework;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddEndpoints()
    .AddEndpointsApiExplorer()
    .AddOpenApi()
    .AddDefaultCorsPolicy();

builder.Services.AddInfrastructure(builder.Environment, builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    DatabaseContext databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    await databaseContext.Database.MigrateAsync();
    await databaseContext.Database.EnsureCreatedAsync();
}

app.UseCors();

app.UseForwardedHeaders();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseExceptionHandler();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
app.UseAuthentication()
    .UseAuthorization();

app.UseMiddleware<StoreTenantMiddleware>();

app.MapDefaultEndpoints()
    .MapEndpoints();

app.UseJwksDiscovery();

app.Run();