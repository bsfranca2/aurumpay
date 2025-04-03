using Microsoft.AspNetCore.Routing;

namespace AurumPay.Application.Common.Interfaces;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
