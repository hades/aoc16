using aoc16;

namespace tests;

public class TestDay05
{
  [Fact(Timeout = 30)]
  public void TestFirstPart()
  {
    var solver = new Day05();
    solver.Presolve("abc");
    Assert.Equal("18f47a30", solver.SolveFirst());
  }

  [Fact(Timeout = 90)]
  public void TestSecondPart()
  {
    var solver = new Day05();
    solver.Presolve("abc");
    Assert.Equal("05ace8e3", solver.SolveSecond());
  }
}