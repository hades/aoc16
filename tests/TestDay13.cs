using aoc16;

namespace tests;

public class TestDay13
{
  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day13();
    solver.Presolve("1364");
    Assert.Equal("86", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day13();
    solver.Presolve("1364");
    Assert.Equal("127", solver.SolveSecond());
  }
}