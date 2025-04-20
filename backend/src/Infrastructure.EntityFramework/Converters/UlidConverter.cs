using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AurumPay.Infrastructure.EntityFramework.Converters;

public class UlidToBytesConverter(ConverterMappingHints? mappingHints) : ValueConverter<Ulid, byte[]>(
    convertToProviderExpression: x => x.ToByteArray(),
    convertFromProviderExpression: x => new Ulid(x),
    mappingHints: DefaultHints.With(mappingHints))
{
    private static readonly ConverterMappingHints DefaultHints = new ConverterMappingHints(size: 16);

    public UlidToBytesConverter() : this(null)
    {
    }
}

public class UlidToStringConverter(ConverterMappingHints? mappingHints) : ValueConverter<Ulid, string>(
    convertToProviderExpression: x => x.ToString(),
    convertFromProviderExpression: x => Ulid.Parse(x),
    mappingHints: DefaultHints.With(mappingHints))
{
    private static readonly ConverterMappingHints DefaultHints = new ConverterMappingHints(size: 26);

    public UlidToStringConverter() : this(null)
    {
    }
}