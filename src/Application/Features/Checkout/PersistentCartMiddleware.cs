// using Microsoft.AspNetCore.Http;
//
// namespace AurumPay.Application.Features.Checkout;
//
// public class PersistentCartMiddleware(RequestDelegate next)
// {
//     private const string CartTokenCookie = "CartToken";
//
//     public async Task InvokeAsync(HttpContext context, CheckoutSessionService sessionService)
//     {
//         Console.WriteLine(context.Request.Path.StartsWithSegments("/checkout"));
//         if (context.Request.Path.StartsWithSegments("/checkout") && context.Items["StoreId"] is Guid storeId)
//         {
//             var sessionId = context.Request.Cookies["CheckoutSession"];
//             Console.WriteLine($"Session Id: {sessionId}");
//             var newSessionId = await sessionService.RestoreSessionAsync(storeId, cartToken);
//             if (newSessionId.HasValue && newSessionId != Guid.Empty)
//             {
//                 context.Items["SessionId"] = newSessionId;
//             }
//         }
//
//         await next(context);
//     }
// }
