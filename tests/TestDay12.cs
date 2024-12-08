using aoc16;

namespace tests;

public class TestDay12
{
  private readonly string data = @"cpy 41 a
inc a
inc a
jnz c 4
dec a
jnz a 2
dec a
".Replace("\r\n", "\n");

  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day12();
    solver.Presolve(data);
    Assert.Equal("42", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day12();
    solver.Presolve(data);
    Assert.Equal("43", solver.SolveSecond());
  }
}