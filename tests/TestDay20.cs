using aoc16;

namespace tests;

public class TestDay20 {
  [Fact]
  public void TestFirstPart() {
    var solver = new Day20();
    solver.Presolve(@"5-8
0-2
4-7
".Replace("\r\n", "\n"));
    Assert.Equal("3", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart() {
    var solver = new Day20();
    solver.Presolve(@"5-8
0-2
4-7
10-4294967295
".Replace("\r\n", "\n"));
    Assert.Equal("2", solver.SolveSecond());
  }
}