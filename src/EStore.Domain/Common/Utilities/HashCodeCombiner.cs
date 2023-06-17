using System.Runtime.CompilerServices;
using EStore.Domain.Common.Models;

namespace EStore.Domain.Common.Utilities;

public struct HashCodeCombiner<TKey, TValue>
    where TKey : ValueObject
    where TValue : ValueObject
{
    private long _combinedHash64;

    public int CombinedHash
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return _combinedHash64.GetHashCode(); }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private HashCodeCombiner(long seed)
    {
        _combinedHash64 = seed;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator int(HashCodeCombiner<TKey, TValue> self)
    {
        return self.CombinedHash;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public HashCodeCombiner<TKey, TValue> Add(int i)
    {
        if (i != 0)
        {
            _combinedHash64 = ((_combinedHash64 << 5) + _combinedHash64) ^ i;
        }

        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public HashCodeCombiner<TKey, TValue> Add(TKey value)
    {
        return Add(value.GetHashCode());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public HashCodeCombiner<TKey, TValue> Add(TValue value)
    {
        return Add(value.GetHashCode());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashCodeCombiner<TKey, TValue> Start()
    {
        return new HashCodeCombiner<TKey, TValue>(0x1505L);
    }
}
