using aoc16;

namespace tests;

public class TestDay11
{


  [Fact]
  public void TestFirstPart()
  {
    string data = @"The first floor contains a hydrogen-compatible microchip, and a lithium-compatible microchip.
The second floor contains a hydrogen generator.
The third floor contains a lithium generator.
The fourth floor contains nothing relevant.
".Replace("\r\n", "\n");
    var solver = new Day11();
    solver.Presolve(data);
    Assert.Equal("11", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    string data = @"The first floor contains a hydrogen-compatible microchip.
The second floor contains a hydrogen generator.
The third floor contains a lithium generator.
The fourth floor contains a lithium-compatible microchip.
".Replace("\r\n", "\n");
    var solver = new Day11();
    solver.Presolve(data);
    Assert.Equal("27", solver.SolveSecond());
  }
}