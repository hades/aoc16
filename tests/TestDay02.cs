namespace tests;

using aoc16;

public class TestDay02
{
    [Theory]
    [InlineData(@"ULL
RRDDD
LURDL
UUUUD", "1985")]
    public void TestFirstPart(string input, string expected)
    {
        var solver = new Day02();
        solver.Presolve(input);
        Assert.Equal(expected, solver.SolveFirst());
    }
    [Theory]
    [InlineData(@"ULL
RRDDD
LURDL
UUUUD", "5DB3")]
    public void TestSecondPart(string input, string expected)
    {
        var solver = new Day02();
        solver.Presolve(input);
        Assert.Equal(expected, solver.SolveSecond());
    }
}