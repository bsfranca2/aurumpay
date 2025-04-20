namespace AurumPay.Domain.Interfaces;

public interface IDeviceIdentityProvider
{
    string GetFingerprint();
}