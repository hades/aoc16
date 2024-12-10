using QuickGraph;
using System.Security.Cryptography;

namespace aoc16;

[ForDay(17)]
public partial class Day17 : Solver {
  private string? longest_path;
  private string? shortest_path;

  public void Presolve(string input) {
    var data = input.Trim();
    var graph = MakeGraph();
    ImplicitBreadthFirstSearchAlgorithm<Vertex, Edge> search = new(graph, [new(0, 0, data)]);
    search.StartVertex += v => {
      if (v.x == 3 && v.y == 3) {
        if (shortest_path is null) {
          shortest_path = v.password[data.Length..];
        }
        if (v.password.Length - data.Length > (longest_path?.Length ?? 0)) {
          longest_path = v.password[data.Length..];
        }
      }
    };
    search.Compute();
  }

  private record Vertex(int x, int y, string password);
  private record Edge(Vertex Source, Vertex Target): IEdge<Vertex>;

  private DelegateImplicitGraph<Vertex, Edge> MakeGraph() => new(GetOutEdges);

  private static readonly (char, int, int, int)[] directions = [
    ('U', 0, -1, 0), ('D', 0, 1, 1), ('L', -1, 0, 2), ('R', 1, 0, 3),
    ];

  private bool GetOutEdges(Vertex arg, out IEnumerable<Edge> result) {
    if (arg.x == 3 && arg.y == 3) {
      result = [];
      return true;
    }
    string hash = GetMd5(arg.password);
    List<Edge> edges = [];
    foreach (var (dir, dx, dy, position) in directions) {
      int x = arg.x + dx;
      int y = arg.y + dy;
      if (x < 0 || y < 0 || x >= 4 || y >= 4) continue;
      if (hash[position] >= 'b' && hash[position] <= 'f') {
        string password = arg.password + dir;
        edges.Add(new(arg, new(x, y, password)));
      }
    }
    result = edges;
    return true;
  }

  private string GetMd5(string password) => Convert.ToHexString(MD5.HashData(System.Text.Encoding.ASCII.GetBytes(password))).ToLower();

  public string SolveFirst() => shortest_path!;
  public string SolveSecond() => longest_path!.Length.ToString();
}