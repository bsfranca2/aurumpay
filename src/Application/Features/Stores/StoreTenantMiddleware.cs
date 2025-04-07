using AurumPay.Application.Infrastructure.Persistence;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AurumPay.Application.Features.Stores;

public class StoreTenantMiddleware(RequestDelegate next)
{
    private const string StoreIdHeader = "X-Store-Id";

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        var storeIdRaw = GetStoreIdFromRequest(context);

        if (Guid.TryParse(storeIdRaw, out var storeId) && storeId != Guid.Empty)
        {
            var exists = await dbContext.Stores.AnyAsync(s => s.Id == storeId);
            if (!exists)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid Store ID");
                return;
            }

            context.Items["StoreId"] = storeId;
        }
        
        await next(context);
    }

    private static string? GetStoreIdFromRequest(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(StoreIdHeader, out var headerValue))
            return headerValue.ToString();

        if (context.Request.Query.TryGetValue("store_id", out var queryValue))
            return queryValue.ToString();

        if (context.Request.Cookies.TryGetValue("StoreId", out var cookieValue))
            return cookieValue;

        return null;
    }
}
