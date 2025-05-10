namespace AurumPay.Application.Customers;

public static class FullNameValidator
{
    public static bool Validate(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return false;
        }

        string[] parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2 && parts[0].Length >= 2;
    }
}