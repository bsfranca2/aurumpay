using AurumPay.Application;
using AurumPay.Application.Infrastructure.Endpoints;
using AurumPay.Application.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpoints()
    .AddEndpointsApiExplorer()
    .AddOpenApi()
    .AddDefaultCorsPolicy();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseCors();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapEndpoints();

app.Run();
