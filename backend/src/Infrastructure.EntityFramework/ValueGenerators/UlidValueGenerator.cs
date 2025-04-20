using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AurumPay.Infrastructure.EntityFramework.ValueGenerators;

public class UlidValueGenerator: ValueGenerator<Guid>
{
    public override Guid Next(EntityEntry entry)
    {
        return Ulid.NewUlid().ToGuid();
    }

    public override bool GeneratesTemporaryValues => false;
}