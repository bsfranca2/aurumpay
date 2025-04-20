using AurumPay.Domain.Stores;

namespace AurumPay.Domain.Interfaces;

public interface IStoreContext
{
    StoreId GetCurrentStoreId();
}