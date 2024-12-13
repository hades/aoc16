using aoc16;

namespace tests;

public class TestDay18 {
  [Theory]
  [InlineData(".^^.^.^^^^", "185")]
  [InlineData(".^^^.^.^^^.^.......^^.^^^^.^^^^..^^^^^.^.^^^..^^.^.^^..^.^..^^...^.^^.^^^...^^.^.^^^..^^^^.....^....", "2013")]
  public void TestFirstPart(string input, string output) {
    var solver = new Day18();
    solver.Presolve(input);
    Assert.Equal(output, solver.SolveFirst());
  }

  [Theory]
  [InlineData(".^^.^.^^^^", "1935478")]
  [InlineData(".^^^.^.^^^.^.......^^.^^^^.^^^^..^^^^^.^.^^^..^^.^.^^..^.^..^^...^.^^.^^^...^^.^.^^^..^^^^.....^....", "20006289")]
  public void TestSecondPart(string input, string output) {
    var solver = new Day18();
    solver.Presolve(input);
    Assert.Equal(output, solver.SolveSecond());
  }
}