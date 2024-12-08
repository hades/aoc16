using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;
using QuickGraph.Algorithms.ShortestPath;
using Point = (int, int);

namespace aoc16;

[ForDay(13)]
public partial class Day13 : Solver {
  private int favourite_number;

  public void Presolve(string input) {
    favourite_number = int.Parse(input.Trim());
  }

  private class PointEdge(Point source, Point target)
      : Edge<Point>(source, target)
      , IEquatable<PointEdge> {
    public bool Equals(PointEdge? other) => Source.Equals(other?.Source) && Target.Equals(other?.Target);
    public override bool Equals(object? obj) => Equals(obj as PointEdge);
    public override int GetHashCode() => HashCode.Combine(Source.GetHashCode(), Target.GetHashCode());
  }

  private class Graph(IEnumerable<Point> vertices, TryFunc<Point, IEnumerable<PointEdge>> tryGetOutEdges)
    : DelegateVertexAndEdgeListGraph<Point, PointEdge>(vertices, tryGetOutEdges) {
  }

  private Graph MakeGraph() => new(AllPoints(), GetNeighbours);

  private readonly Point[] directions = [(1, 0), (-1, 0), (0, 1), (0, -1)];

  private bool IsWall(int x, int y) {
    long is_wall_number = x * x + 3 * x + 2 * x * y + y + y * y + favourite_number;
    return (long.PopCount(is_wall_number) % 2) == 1;
  }

  private bool GetNeighbours(Point from, out IEnumerable<PointEdge> result_enumerable) {
    List<PointEdge> result = [];
    foreach (var (dx, dy) in directions) {
      int x = from.Item1 + dx;
      int y = from.Item2 + dy;
      if (x < 0 || y < 0) continue;
      if (!IsWall(x, y)) result.Add(new PointEdge(from, (x, y)));
    }
    result_enumerable = result;
    return true;
  }

  private static IEnumerable<Point> AllPoints() {
    for (int i = 0; i <= 100; i++) {
      for (int j = 0; j <= 100; j++) {
        yield return (i, j);
      }
    }
  }

  public string SolveFirst() {
    var finder = MakeGraph().ShortestPathsAStar(_ => 1, point => point.Item1 + point.Item2 - 2, (1, 1));
    if (!finder((31, 39), out var path)) throw new InvalidDataException("unable to find path");
    return path.Count().ToString();
  }

  public string SolveSecond() {
    var graph = MakeGraph();
    FrontierBreadthFirstSearchAlgorithm<Point, PointEdge> algo = new(graph, [(1, 1)]);
    int count = 0;
    algo.ExamineFrontierVertex += vertex => {
      if (algo.Height > 50) {
        algo.Abort();
        return;
      }
      count += 1;
    };
    algo.Compute();
    return count.ToString();
  }
}