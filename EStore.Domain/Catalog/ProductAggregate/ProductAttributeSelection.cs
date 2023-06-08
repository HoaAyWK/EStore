using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Collections;
using EStore.Domain.Common.Utilities;
using Newtonsoft.Json;

namespace EStore.Domain.Catalog.ProductAggregate;

public sealed class ProductAttributeSelection : IEquatable<ProductAttributeSelection>
{
    private string? _rawAttributes;
    private readonly MultiMap<ProductAttributeId, ProductAttributeValueId> _attributes = new();
    private bool _dirty = true;

    public IEnumerable<KeyValuePair<ProductAttributeId, ICollection<ProductAttributeValueId>>> AttributesMap =>
        _attributes;

    private ProductAttributeSelection(string? rawAttributes)
    {
        _rawAttributes = !string.IsNullOrEmpty(rawAttributes) ? rawAttributes.Trim() : rawAttributes;
        _attributes = GetFromJson() ?? new();
    }

    public static ProductAttributeSelection Create(string? rawAttributes)
    {
        return new(rawAttributes);
    }

    private MultiMap<ProductAttributeId, ProductAttributeValueId>? GetFromJson()
    {
        if (_rawAttributes is null
            || _rawAttributes.Equals(string.Empty)
            || _rawAttributes.Length <= 2)
        {
            return new MultiMap<ProductAttributeId, ProductAttributeValueId>();
        }

        if (_rawAttributes[0] != '{' && _rawAttributes[0] != '[')
        {
            throw new ArgumentException("Raw attributes must be in JSON format: " + _rawAttributes);
        }

        try
        {
            return JsonConvert.DeserializeObject<MultiMap<ProductAttributeId, ProductAttributeValueId>>(_rawAttributes);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to deserialize attributes from JSON: " + _rawAttributes, ex);
        }
    }

    public void AddAttributeValue(ProductAttributeId attributeId, ProductAttributeValueId value)
    {
        _attributes.Add(attributeId, value);
        _dirty = true;
    }

    public string? AsJson()
    {
        if (!string.IsNullOrEmpty(_rawAttributes) && !_dirty)
        {
            return _rawAttributes;
        }

        if (_attributes.Count == 0)
        {
            return null;
        }

        try
        {
            var json = JsonConvert.SerializeObject(_attributes);

            _dirty = false;
            _rawAttributes = json;

            return json;
        }
        catch (Exception ex)
        {
            throw new JsonSerializationException("Failed to serialize JSON string from: " + nameof(_attributes), ex);
        }
    }

    public override int GetHashCode()
    {
        var combiner = HashCodeCombiner<ProductAttributeId, ProductAttributeValueId>.Start();

        foreach (var attribute in _attributes)
        {
            combiner.Add(attribute.GetHashCode());

            foreach (var value in attribute.Value)
            {
                combiner.Add(value.GetHashCode());
            }
        }

        return combiner.CombinedHash;
    }

    public static bool operator ==(
        ProductAttributeSelection left,
        ProductAttributeSelection right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(
        ProductAttributeSelection left,
        ProductAttributeSelection right)
    {
        return Equals(left, right);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ProductAttributeSelection);
    }

    public bool Equals(ProductAttributeSelection? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (this._attributes.Count != other._attributes.Count)
        {
            return false;
        }

        foreach (var pair in this._attributes)
        {
            if (!other._attributes.ContainsKey(pair.Key))
            {
                return false;
            }

            var values = pair.Value;
            var otherValues = other._attributes[pair.Key];

            if (values.Count != otherValues.Count)
            {
                return false;
            }

            foreach (var value in values)
            {
                if (!otherValues.Any(x => x == value))
                {
                    return false;
                }
            }

        }
        return true;
    }
}
