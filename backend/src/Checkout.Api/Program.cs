using AurumPay.Checkout.Api;
using AurumPay.Checkout.Api.Infrastructure.Endpoints;
using AurumPay.Infrastructure.EntityFramework;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddEndpoints()
    .AddEndpointsApiExplorer()
    .AddOpenApi()
    .AddDefaultCorsPolicy();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    await databaseContext.Database.MigrateAsync();
    await databaseContext.Database.EnsureCreatedAsync();
}

app.UseCors();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication()
    .UseAuthorization();

app.UseMiddleware<StoreTenantMiddleware>();

app.MapDefaultEndpoints()
    .MapEndpoints();

app.Run();
