using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteWheel<T>
{
    public RouletteWheel(List<T> items, List<float> chances)
    {
        SetCachedDictionaryFromLists(items, chances);
    }

    private Dictionary<T, float> _cachedDictionary = new();

    private float _cachedSum;

    public void SetCachedDictionaryFromLists(List<T> items, List<float> chances)
    {
        _cachedDictionary = new Dictionary<T, float>();
        _cachedSum = 0;

        for (int i = 0; i < items.Count; i++)
        {
            var chance = chances[i];
            _cachedDictionary.Add(items[i], chance);
            _cachedSum += chance;
        }
    }

    public T RunWithCached()
    {
        if (_cachedDictionary == null)
            return default;

        var random = Random.Range(0, _cachedSum);

        foreach (var item in _cachedDictionary)
        {
            random -= item.Value;
            if (random <= 0)
            {
                return item.Key;
            }
        }

        return default;
    }
}
