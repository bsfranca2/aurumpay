using MediatR;

namespace AurumPay.Core;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}