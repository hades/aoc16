using aoc16;

namespace tests;

public class TestDay10
{
  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day10();
    solver.Presolve(@"value 61 goes to bot 2
bot 2 gives low to bot 1 and high to bot 0
value 17 goes to bot 1
bot 1 gives low to output 1 and high to bot 0
bot 0 gives low to output 2 and high to output 0
value 16 goes to bot 2
");
    Assert.Equal("0", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day10();
    solver.Presolve(@"value 61 goes to bot 2
bot 2 gives low to bot 1 and high to bot 0
value 17 goes to bot 1
bot 1 gives low to output 1 and high to bot 0
bot 0 gives low to output 2 and high to output 0
value 16 goes to bot 2
");
    Assert.Equal("16592", solver.SolveSecond());
  }
}