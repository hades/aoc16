using QuickGraph;
using QuickGraph.Algorithms;
using System.Collections.Immutable;

namespace aoc16;

public sealed class FrontierBreadthFirstSearchAlgorithm<Vertex, Edge>(
  IVertexListGraph<Vertex, Edge> graph, IEnumerable<Vertex> start)
  : AlgorithmBase<IVertexListGraph<Vertex, Edge>>(graph)
  where Edge : IEdge<Vertex> {
  private readonly ImmutableList<Vertex> start = start.ToImmutableList();

  private long h = 0;

  public long Height { get => h; }

  public event Action<Vertex>? ExamineFrontierVertex;

  protected override void InternalCompute() {
    Dictionary<Vertex, GraphColor> colours = [];
    ImmutableList<Vertex> frontier = start;
    while (frontier.Count > 0) {
      List<Vertex> new_frontier = [];
      foreach (var v in frontier) {
        ExamineFrontierVertex?.Invoke(v);
        colours[v] = GraphColor.Black;
        foreach (var e in VisitedGraph.OutEdges(v)) {
          if (Services.CancelManager.IsCancelling) return;

          var t = e.Target;
          var colour = colours.GetValueOrDefault(t, GraphColor.White);
          if (colour == GraphColor.White) {
            colours[t] = GraphColor.Gray;
            new_frontier.Add(t);
          }
        }
      }
      frontier = new_frontier.ToImmutableList();
      h++;
    }
  }
}
