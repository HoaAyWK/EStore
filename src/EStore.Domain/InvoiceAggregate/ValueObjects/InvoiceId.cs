using System.Text.Json.Serialization;
using EStore.Domain.Common.Models;

namespace EStore.Domain.InvoiceAggregate.ValueObjects;

public sealed class InvoiceId : ValueObject
{
    public Guid Value { get; }

    private InvoiceId(Guid value)
    {
        Value = value;
    }

    [JsonConstructor]
    private InvoiceId(string value)
    {
        Value = new Guid(value);
    }

    public static InvoiceId Create(Guid value)
    {
        return new(value);
    }

    public static InvoiceId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}