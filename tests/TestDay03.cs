namespace tests;

using aoc16;

public class TestDay03
{
    [Theory]
    [InlineData(@"3 4 5", "1")]
    [InlineData(@"3 4 8", "0")]
    [InlineData(@"3 8 4", "0")]
    [InlineData(@"8 3 4", "0")]
    [InlineData(@"8 4 3", "0")]
    [InlineData(@"4 8 3", "0")]
    [InlineData(@"4 3 8", "0")]
    public void TestFirstPart(string input, string expected)
    {
        var solver = new Day03();
        solver.Presolve(input);
        Assert.Equal(expected, solver.SolveFirst());
    }

    [Fact]
    public void TestSecondPart()
    {
        var solver = new Day03();
        solver.Presolve(@"3 3 3
    4 4 8
    5 8 4
    8 8 4
    3 4 8
    4 3 3
    4 3 4
    3 5 5
    8 4 3");
        Assert.Equal("3", solver.SolveSecond());
    }
}