using QuickGraph;
using QuickGraph.Algorithms;
using System.Collections.Immutable;

namespace aoc16;

public sealed class ImplicitBreadthFirstSearchAlgorithm<Vertex, Edge>(
  IImplicitGraph<Vertex, Edge> graph, ImmutableList<Vertex> start)
  : AlgorithmBase<IImplicitGraph<Vertex, Edge>>(graph)
  , IVertexPredecessorRecorderAlgorithm<Vertex, Edge>
  , IVertexColorizerAlgorithm<Vertex, Edge>
  where Edge : IEdge<Vertex>
  where Vertex : notnull {
  private readonly Dictionary<Vertex, GraphColor> colours = [];

  public event VertexAction<Vertex>? StartVertex;
  public event VertexAction<Vertex>? FinishVertex;
  public event EdgeAction<Vertex, Edge>? TreeEdge;

  public GraphColor GetVertexColor(Vertex v) {
    return colours.GetValueOrDefault(v, GraphColor.White);
  }

  protected override void InternalCompute() {
    ImmutableList<Vertex> frontier = start;
    foreach (var v in frontier) {
      StartVertex?.Invoke(v);
    }
    while (frontier.Count > 0) {
      List<Vertex> new_frontier = [];
      foreach (var v in frontier) {
        colours[v] = GraphColor.Black;
        foreach (var e in VisitedGraph.OutEdges(v)) {
          if (Services.CancelManager.IsCancelling) return;

          var t = e.Target;
          var colour = colours.GetValueOrDefault(t, GraphColor.White);
          if (colour == GraphColor.White) {
            TreeEdge?.Invoke(e);
            colours[t] = GraphColor.Gray;
            new_frontier.Add(t);
          }
        }
        FinishVertex?.Invoke(v);
      }
      frontier = new_frontier.ToImmutableList();
      foreach (var v in frontier) {
        StartVertex?.Invoke(v);
      }
    }
  }
}