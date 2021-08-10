using System.Collections.Generic;

public static class Extensions {
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value) {
        key = pair.Key;
        value = pair.Value;
    }

    public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key,
        TValue defaultValue = default) {
        var gotValue = dictionary.TryGetValue(key, out var value);
        if (!gotValue) {
            value = defaultValue;
        }

        return value;
    }
}
