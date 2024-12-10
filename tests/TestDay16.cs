using aoc16;

namespace tests;

public class TestDay16
{
  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day16();
    solver.Presolve("01000100010010111");
    Assert.Equal("10010010110011010", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day16();
    solver.Presolve("01000100010010111");
    Assert.Equal("01010100101011100", solver.SolveSecond());
  }
}