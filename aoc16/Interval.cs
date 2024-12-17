namespace aoc16;

public enum EndSpecifier {
  Open, Closed, Infinity
}

public class Interval<T>: IComparable<Interval<T>>, IEquatable<Interval<T>>
  where T: IComparable<T> {
  public static Interval<T> Empty { get => new(); }
  public static Interval<T> LessThan(T upper) =>
    new(default, upper, EndSpecifier.Infinity, EndSpecifier.Open);
  public static Interval<T> LessThanOrEquals(T upper) =>
    new(default, upper, EndSpecifier.Infinity, EndSpecifier.Closed);
  public static Interval<T> GreaterThan(T lower) =>
    new(lower, default, EndSpecifier.Open, EndSpecifier.Infinity);
  public static Interval<T> GreaterThanOrEquals(T lower) =>
    new(lower, default, EndSpecifier.Closed, EndSpecifier.Infinity);
  public static Interval<T> Open(T lower, T upper) =>
    new(lower, upper, EndSpecifier.Open, EndSpecifier.Open);
  public static Interval<T> OpenClosed(T lower, T upper) =>
    new(lower, upper, EndSpecifier.Open, EndSpecifier.Closed);
  public static Interval<T> ClosedOpen(T lower, T upper) =>
    new(lower, upper, EndSpecifier.Closed, EndSpecifier.Open);
  public static Interval<T> Closed(T lower, T upper) =>
    new(lower, upper, EndSpecifier.Closed, EndSpecifier.Closed);
  public static Interval<T> Singleton(T value) =>
    new(value, value, EndSpecifier.Closed, EndSpecifier.Closed);

  public Interval(T lower, T upper, EndSpecifier left, EndSpecifier right)
    : this([new Atomic(lower, upper, left, right)]) {}

  public bool IsEmpty { get => atomics.Count == 0; }
  public EndSpecifier Left { get => IsEmpty ? EndSpecifier.Open : atomics[0].Left; }
  public EndSpecifier Right { get => IsEmpty ? EndSpecifier.Open : atomics[^1].Right; }
  public T Lower {
    get => IsEmpty ? default : atomics[0].Lower;
  }
  public T Upper {
    get => IsEmpty ? default : atomics[^1].Upper;
  }

  public int CompareTo(Interval<T>? other) {
    if (other is null || other.IsEmpty) return IsEmpty ? 0 : 1;
    if (IsEmpty) return -1;
    return atomics[0].CompareTo(other.atomics[0]);
  }

  public bool Equals(Interval<T>? other) {
    if (other is null || other.IsEmpty) return IsEmpty;
    return atomics.SequenceEqual(other.atomics);
  }

  public bool Contains(T value) {
    foreach (var interval in atomics) {
      bool left_matches = interval.Left is EndSpecifier.Infinity || (
        interval.Left is EndSpecifier.Closed ? value.CompareTo(interval.Lower) != -1 : value.CompareTo(interval.Lower) == 1);
      if (!left_matches) continue;
      bool right_matches = interval.Right is EndSpecifier.Infinity || (
        interval.Right is EndSpecifier.Closed ? value.CompareTo(interval.Upper) != 1 : value.CompareTo(interval.Upper) == -1);
      if (!right_matches) continue;
      return true;
    }
    return false;
  }

  public static Interval<T> operator -(Interval<T> left, Interval<T> right) => left & ~right;

  public static Interval<T> operator |(Interval<T> left, Interval<T> right) => new([left, right]);

  public static Interval<T> operator &(Interval<T> left, Interval<T> right) => ~((~left) | (~right));

  public static Interval<T> operator~(Interval<T> arg) {
    if (arg.atomics.Count == 0) {
      return new([new Atomic(default, default, EndSpecifier.Infinity, EndSpecifier.Infinity)]);
    }
    if (arg.atomics[0].Left is EndSpecifier.Infinity && arg.atomics[0].Right is EndSpecifier.Infinity) return new();
    List<Atomic> result = [];
    if (arg.atomics[0].Left is not EndSpecifier.Infinity) {
      result.Add(new(default, arg.atomics[0].Lower, EndSpecifier.Infinity, Invert(arg.atomics[0].Left)));
    }
    var pending = (arg.atomics[0].Upper, Invert(arg.atomics[0].Right));
    for (int i = 1; i < arg.atomics.Count; i++) {
      result.Add(new(pending.Item1, arg.atomics[i].Lower, pending.Item2, Invert(arg.atomics[i].Left)));
      pending = (arg.atomics[i].Upper, Invert(arg.atomics[i].Right));
    }
    if (pending.Item2 is not EndSpecifier.Infinity) {
      result.Add(new(pending.Item1, default, pending.Item2, EndSpecifier.Infinity));
    }
    return new(result);
  }

  public IEnumerable<T> EnumerateDiscrete(Func<T, T> increment) {
    foreach (var interval in atomics) {
      if (interval.Left is EndSpecifier.Infinity) throw new ArgumentException();
      T value = interval.Lower;
      if (interval.Left is EndSpecifier.Closed) yield return value;
      value = increment(value);
      int compare = value.CompareTo(interval.Upper);
      while (compare < 0) {
        yield return value;
        value = increment(value);
        compare = value.CompareTo(interval.Upper);
      }
      if (compare == 0 && interval.Right is EndSpecifier.Closed) yield return value;
    }
  }

  public IEnumerable<Interval<T>> EnumerateContiguousIntervals() {
    foreach (var interval in atomics) {
      yield return new([interval]);
    }
  }

  private record struct Atomic(T Lower, T Upper, EndSpecifier Left, EndSpecifier Right)
    : IComparable<Atomic> {
    public readonly int CompareTo(Atomic other) {
      if (Left is EndSpecifier.Infinity) return other.Left is EndSpecifier.Infinity ? 0 : -1;
      if (other.Left is EndSpecifier.Infinity) return 1;
      var compare = Lower.CompareTo(other.Lower);
      if (compare != 0) return compare;
      if (Left == other.Left) return 0;
      if (Left is EndSpecifier.Closed) return -1;
      return 1;
    }
  }

  private readonly List<Atomic> atomics = [];

  private Interval() { }

  private Interval(IEnumerable<Atomic> intervals) {
    atomics = [.. intervals];
  }

  private Interval(IEnumerable<Interval<T>> intervals) {
    atomics = [.. intervals.SelectMany(interval => interval.atomics)];
    if (atomics.Count == 0) return;
    atomics.Sort();
    for (int i = 0; i < atomics.Count - 1;) {
      var left = atomics[i];
      var right = atomics[i + 1];
      if (!IsMergeable(left, right)) {
        i++;
        continue;
      }
      var union = new Atomic(left.Lower, right.Upper, left.Left, right.Right);
      if (right.Right is EndSpecifier.Infinity || left.Right is EndSpecifier.Infinity) {
        union.Upper = default;
        union.Right = EndSpecifier.Infinity;
      } else {
        var compare_upper = right.Upper.CompareTo(left.Upper);
        if (compare_upper == 0) {
          union.Right = left.Right is EndSpecifier.Closed || right.Right is EndSpecifier.Closed ?
            EndSpecifier.Closed : EndSpecifier.Open;
        } else if (compare_upper < 0) {
          union.Right = left.Right;
          union.Upper = left.Upper;
        }
      }
      atomics.RemoveAt(i);
      atomics.RemoveAt(i);
      atomics.Insert(i, union);
    }
  }

  private static bool IsMergeable(Atomic left, Atomic right) {
    if (left.Left is EndSpecifier.Infinity || right.Right is EndSpecifier.Infinity) {
      if (left.Right is EndSpecifier.Infinity || right.Left is EndSpecifier.Infinity) return true;
      var compare_lr = left.Upper.CompareTo(right.Lower);
      if (compare_lr == 0) return left.Right is EndSpecifier.Closed || right.Left is EndSpecifier.Closed;
      return compare_lr > 0;
    }
    var compare_rl = right.Upper.CompareTo(left.Lower);
    if (compare_rl == 0) return left.Left is EndSpecifier.Closed || right.Right is EndSpecifier.Closed;
    if (compare_rl < 0) return false;
    if (left.Right is EndSpecifier.Infinity || right.Left is EndSpecifier.Infinity) return true;
    var compare_ll = left.Upper.CompareTo(right.Lower);
    if (compare_ll == 0) return left.Right is EndSpecifier.Closed || right.Left is EndSpecifier.Closed;
    return compare_ll > 0;
  }

  private static EndSpecifier Invert(EndSpecifier arg) => arg switch {
    EndSpecifier.Open => EndSpecifier.Closed,
    EndSpecifier.Closed => EndSpecifier.Open,
    EndSpecifier.Infinity => EndSpecifier.Infinity,
    _ => throw new ArgumentException(),
  };
}

public static class IntervalExtensions {
#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
  public static bool IsLowerBoundOf<T>(this T value, Interval<T> interval) where T : IComparable<T> =>
    interval.Left switch {
      EndSpecifier.Infinity => false,
      EndSpecifier.Open => value.CompareTo(interval.Lower) <= 0,
      EndSpecifier.Closed => value.CompareTo(interval.Lower) < 0,
    };
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
}
