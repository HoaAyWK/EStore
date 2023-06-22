using EStore.Domain.Common.Collections;
using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.Common.Utilities;

public sealed class AttributeSelection<TKey, TValue>
    : IEquatable<AttributeSelection<TKey, TValue>>
    where TKey : ValueObject
    where TValue : ValueObject
{
    private string? _rawAttributes;
    private readonly MultiMap<TKey, TValue> _attributes = new();
    private bool _dirty = true;

    public IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> AttributesMap =>
        _attributes;

    private AttributeSelection(string? rawAttributes)
    {
        _rawAttributes = !string.IsNullOrEmpty(rawAttributes) ? rawAttributes.Trim() : rawAttributes;
        _attributes = GetFromJson() ?? new();
    }

    public static AttributeSelection<TKey, TValue> Create(string? rawAttributes)
    {
        return new(rawAttributes);
    }

    private MultiMap<TKey, TValue>? GetFromJson()
    {
        if (_rawAttributes is null
            || _rawAttributes.Equals(string.Empty)
            || _rawAttributes.Length <= 2)
        {
            return new MultiMap<TKey, TValue>();
        }

        if (_rawAttributes[0] != '{' && _rawAttributes[0] != '[')
        {
            throw new ArgumentException("Raw attributes must be in JSON format: " + _rawAttributes);
        }

        try
        {
            return JsonConvert.DeserializeObject<MultiMap<TKey, TValue>>(_rawAttributes);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to deserialize attributes from JSON: " + _rawAttributes, ex);
        }
    }

    public void AddAttributeValue(TKey key, TValue value)
    {
        _attributes.Add(key, value);
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
        var combiner = HashCodeCombiner<TKey, TValue>.Start();

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
        AttributeSelection<TKey, TValue> left,
        AttributeSelection<TKey, TValue> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(
        AttributeSelection<TKey, TValue> left,
        AttributeSelection<TKey, TValue> right)
    {
        return Equals(left, right);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as AttributeSelection<TKey, TValue>);
    }

    public bool Equals(AttributeSelection<TKey, TValue>? other)
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

