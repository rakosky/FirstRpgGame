using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
public static class ListExtensions
{
    public static T? MaxDefaultRandom<T, TKey>(this ICollection<T> source, Func<T, TKey> selector)
        where TKey : IComparable<TKey>
    {
        if (source == null || source.Count == 0)
            return default;

        var sorted = source.OrderByDescending(selector);
        
        T max = sorted.ElementAt(0);
        TKey maxKey = selector(max);

        var filtered = sorted.Where(x => selector(x).CompareTo(selector(max)) == 0);

        return filtered.ElementAt(UnityEngine.Random.Range(0, filtered.Count()));
    }
}