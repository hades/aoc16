using System.Collections.Immutable;

namespace aoc16;

[ForDay(1)]
public class Day01 : Solver
{
  private readonly ImmutableList<Tuple<int, int>> DIRECTION_MAP = ImmutableList.Create(
    new Tuple<int, int>(0, 1), new Tuple<int, int>(1, 0), new Tuple<int, int>(0, -1),
    new Tuple<int, int>(-1, 0)
  );

  private ImmutableList<Tuple<char, int>> directions = ImmutableList<Tuple<char, int>>.Empty;

  public void Presolve(string input)
  {
    directions = input.Split(", ").Select(t => ParseDirection(t)).ToImmutableList();
  }

  public string SolveFirst()
  {
    int i = 0, j = 0;
    var dir = 0;
    foreach (var (rotation, steps) in directions)
    {
      if (rotation == 'L') dir += 3;
      if (rotation == 'R') dir += 1;
      dir %= 4;
      var (di, dj) = DIRECTION_MAP[dir];
      i += di * steps;
      j += dj * steps;
    }

    return (Math.Abs(i) + Math.Abs(j)).ToString();
  }

  public string SolveSecond()
  {
    var visited = new HashSet<Tuple<int, int>>();
    int i = 0, j = 0;
    var dir = 0;
    foreach (var (rotation, steps) in directions)
    {
      if (rotation == 'L') dir += 3;
      if (rotation == 'R') dir += 1;
      dir %= 4;
      var (di, dj) = DIRECTION_MAP[dir];
      for (var step = 0; step < steps; step++)
      {
        i += di;
        j += dj;
        var key = Tuple.Create(i, j);
        if (visited.Contains(key)) return (Math.Abs(i) + Math.Abs(j)).ToString();
        visited.Add(key);
      }
    }

    throw new InvalidOperationException("no locations were visited twice");
  }

  private static Tuple<char, int> ParseDirection(string t)
  {
    return new Tuple<char, int>(t[0], int.Parse(t.Substring(1)));
  }
}