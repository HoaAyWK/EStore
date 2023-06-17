using System.Collections;
using EStore.Domain.Common.Models;
using Newtonsoft.Json;

namespace EStore.Domain.Common.Collections;

[JsonConverter(typeof(MultiMapJsonConverter))]
public class MultiMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, ICollection<TValue>>>
    where TKey : notnull
{
    private readonly IDictionary<TKey, ICollection<TValue>> _dict;
    private readonly Func<IEnumerable<TValue>, ICollection<TValue>> _collectionCreator;

    internal readonly static Func<IEnumerable<TValue>, ICollection<TValue>> DefaultCollectionCreator =
        x => new List<TValue>(x ?? Enumerable.Empty<TValue>());

    public MultiMap() : this(EqualityComparer<TKey>.Default)
    {
    }

    public MultiMap(IEqualityComparer<TKey> comparer)
    {
        _dict = new Dictionary<TKey, ICollection<TValue>>(comparer ?? EqualityComparer<TKey>.Default);
        _collectionCreator = DefaultCollectionCreator;
    }

    public MultiMap(Func<IEnumerable<TValue>, ICollection<TValue>> collectionCreator)
        : this(new Dictionary<TKey, ICollection<TValue>>(), collectionCreator)
    {
    }

    public MultiMap(
        IEqualityComparer<TKey> comparer,
        Func<IEnumerable<TValue>, ICollection<TValue>> collectionCreator)
        : this(new Dictionary<TKey, ICollection<TValue>>(comparer ?? EqualityComparer<TKey>.Default), collectionCreator)
    {
    }

    internal MultiMap(
        IDictionary<TKey, ICollection<TValue>> dictionary,
        Func<IEnumerable<TValue>, ICollection<TValue>> collectionCreator)
    {
        _dict = dictionary;
        _collectionCreator = collectionCreator;
    }

    public MultiMap(IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> items)
        : this(items, null)
    {
    }

    public MultiMap(IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> items, IEqualityComparer<TKey>? comparer)
    {
        _dict = new Dictionary<TKey, ICollection<TValue>>(comparer ?? EqualityComparer<TKey>.Default);

        if (items is not null)
        {
            foreach (var kvp in items)
            {
                _dict[kvp.Key] = CreateCollection(kvp.Value);
            }
        }
    }

    public int Count
    {
        get => _dict.Keys.Count;
    }

    public int TotalValueCount
    {
        get => _dict.Values.Sum(x => x.Count);
    }

    public virtual ICollection<TValue> CreateCollection(IEnumerable<TValue>? values)
    {
        return (_collectionCreator ?? DefaultCollectionCreator)(values ?? Enumerable.Empty<TValue>());
    }

    public virtual ICollection<TValue> this[TKey key]
    {
        get
        {
            if (!_dict.ContainsKey(key))
            {
                _dict[key] = CreateCollection(null);
            }

            return _dict[key];
        }
    }

    public virtual ICollection<TKey> Keys
    {
        get => _dict.Keys;
    }

    public virtual ICollection<ICollection<TValue>> Values
    {
        get => _dict.Values;
    }

    public virtual void Add(TKey key, TValue value)
    {
        this[key].Add(value);
    }

    public virtual bool Remove(TKey key, TValue value)
    {
        if (_dict.TryGetValue(key, out var values))
        {
            var removed = values.Remove(value);

            if (removed && values.Count == 0)
            {
                _dict.Remove(key);
            }

            return removed;
        }

        return false;
    }

    public virtual bool RemoveAll(TKey key)
    {
        return _dict.Remove(key);
    }

    public virtual void Clear()
    {
        _dict.Clear();
    }

    public virtual bool ContainsKey(TKey key)
    {
        return _dict.ContainsKey(key);
    }

    public virtual bool TryGetValue(TKey key, out ICollection<TValue>? values)
    {
        return _dict.TryGetValue(key, out values);
    }

    public virtual bool ContainsValue(TKey key, TValue value)
    {
        return _dict.TryGetValue(key, out var values) && values.Contains(value);
    }

    public IDictionary<TKey, ICollection<TValue>> AsDictionary() => _dict;

    public virtual IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
    {
        return _dict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
