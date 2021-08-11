using System.Collections.Generic;
using UnityEngine;

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
    
    public static Color WithAlpha(this Color color, float alpha) {
        var newColor = color;
        newColor.a = alpha;
        return newColor;
    }

    public static Vector2 DegreesToVector(this float degrees) {
        var rad = Mathf.Deg2Rad * degrees;
        return new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
    }
}
