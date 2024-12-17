namespace aoc16;

[ForDay(20)]
public partial class Day20 : Solver {
  private Interval<long> blocked = Interval<long>.LessThan(0) | Interval<long>.GreaterThan(4294967295);

  public void Presolve(string input) {
    var data = input.Trim().Split("\n")
      .Select(line => line.Split("-"))
      .Select(pair => (long.Parse(pair[0]), long.Parse(pair[1]))).ToArray();
    foreach (var (from, to) in data) {
      blocked |= Interval<long>.Closed(from, to);
    }
  }

  public string SolveFirst() => (~blocked).EnumerateDiscrete(i => i + 1).First().ToString();

  public string SolveSecond() => (~blocked).EnumerateContiguousIntervals()
    .Select(interval =>
    interval.Upper - interval.Lower
      + (interval.Right is EndSpecifier.Closed ? 1 : 0)
      - (interval.Left is EndSpecifier.Open ? 1 : 0))
    .Sum().ToString();
}