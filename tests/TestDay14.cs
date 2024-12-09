using aoc16;

namespace tests;

public class TestDay14
{
  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day14();
    solver.Presolve("abc");
    Assert.Equal("22728", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day14();
    solver.Presolve("abc");
    Assert.Equal("22551", solver.SolveSecond());
  }
}