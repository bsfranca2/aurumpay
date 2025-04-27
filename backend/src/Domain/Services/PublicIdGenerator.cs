using NanoidDotNet;

namespace AurumPay.Domain.Services;

public static class PublicIdGenerator
{
    private static readonly string Alphabet = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";

    public static string GeneratePublicId(int length = 10)
    {
        return Nanoid.Generate(Alphabet, length);
    }
}