using aoc16;

namespace tests;

public class TestDay15
{
  private const string data = @"Disc #1 has 5 positions; at time=0, it is at position 4.
Disc #2 has 2 positions; at time=0, it is at position 1.
";

  private const string real_data = @"Disc #1 has 13 positions; at time=0, it is at position 10.
Disc #2 has 17 positions; at time=0, it is at position 15.
Disc #3 has 19 positions; at time=0, it is at position 17.
Disc #4 has 7 positions; at time=0, it is at position 1.
Disc #5 has 5 positions; at time=0, it is at position 0.
Disc #6 has 3 positions; at time=0, it is at position 1.
";

  [Theory]
  [InlineData(data, "5")]
  [InlineData(real_data, "203660")]
  public void TestFirstPart(string input, string output)
  {
    var solver = new Day15();
    solver.Presolve(input.Replace("\r\n", "\n"));
    Assert.Equal(output, solver.SolveFirst());
  }

  [Theory]
  [InlineData(data, "85")]
  [InlineData(real_data, "2408135")]
  public void TestSecondPart(string input, string output)
  {
    var solver = new Day15();
    solver.Presolve(input.Replace("\r\n", "\n"));
    Assert.Equal(output, solver.SolveSecond());
  }
}