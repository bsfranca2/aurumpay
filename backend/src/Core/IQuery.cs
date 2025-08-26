using MediatR;

namespace AurumPay.Core;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}