using System.Diagnostics.CodeAnalysis;

namespace aoc16;

public class IntervalDictionary<K, V>
  where K : IComparable<K>
  where V : IEquatable<V> {
  public V this[K point] {
    get {
      if (!TryGetValue(point, out var result)) {
        throw new KeyNotFoundException($"key not found in IntervalDictionary: {point}");
      }
      return result;
    }
  }

  public V this[Interval<K> interval] {
    set {
      throw new NotImplementedException();
    }
  }

  public bool TryGetValue(K point, [MaybeNullWhen(false)] out V result) {
    foreach (var (interval, value) in data) {
      if (point.IsLowerBoundOf(interval)) break;
      if (interval.Contains(point)) {
        result = value;
        return true;
      }
    }
    result = default;
    return false;
  }

  public void SetValue(Interval<K> interval, V value) {
    if (interval.IsEmpty) return;
    List<Interval<K>> keys_to_remove = [];
    List<KeyValuePair<Interval<K>, V>> values_to_add = [];
    bool merge_with_existing = false;
    foreach (var (existing_interval, existing_data) in data) {
      if (existing_data.Equals(value)) {
        merge_with_existing = true;
        keys_to_remove.Add(existing_interval);
        values_to_add.Add(new(existing_interval | interval, value));
      } else if ((existing_interval & interval) is { IsEmpty: false }) {
        keys_to_remove.Add(existing_interval);
        var remaining_interval = existing_interval - interval;
        if (!remaining_interval.IsEmpty) {
          values_to_add.Add(new(remaining_interval, existing_data));
        }
      }
    }
    if (!merge_with_existing) {
      values_to_add.Add(new(interval, value));
    }
    foreach (var key in keys_to_remove) data.Remove(key);
    foreach (var kv in values_to_add) data.Add(kv.Key, kv.Value);
  }

  private readonly SortedDictionary<Interval<K>, V> data = [];
}
