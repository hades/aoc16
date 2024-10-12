using aoc16;

namespace tests;

public class TestDay09
{
  [Theory]
  [InlineData("ADVENT", "6")]
  [InlineData("A(1x5)BC", "7")]
  [InlineData("(3x3)XYZ", "9")]
  [InlineData("A(2x2)BCD(2x2)EFG", "11")]
  [InlineData("(6x1)(1x3)A", "6")]
  [InlineData("X(8x2)(3x3)ABCY", "18")]
  public void TestFirstPart(string input, string expected)
  {
    var solver = new Day09();
    solver.Presolve(input);
    Assert.Equal(expected, solver.SolveFirst());
  }

  [Theory]
  [InlineData("ADVENT", "6")]
  [InlineData("A(1x5)BC", "7")]
  [InlineData("(3x3)XYZ", "9")]
  [InlineData("X(8x2)(3x3)ABCY", "20")]
  [InlineData("(27x12)(20x12)(13x14)(7x10)(1x12)A", "241920")]
  [InlineData("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN", "445")]
  public void TestSecondPart(string input, string expected)
  {
    var solver = new Day09();
    solver.Presolve(input);
    Assert.Equal(expected, solver.SolveSecond());
  }
}