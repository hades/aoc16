namespace tests;

using aoc16;

public class TestDay01
{
    [Theory]
    [InlineData("R2, L3", 5)]
    [InlineData("R2, R2, R2", 2)]
    [InlineData("R5, L5, R5, R3", 12)]
    public void TestFirstPart(string input, int expected)
    {
        var solver = new Day01();
        solver.Presolve(input);
        Assert.Equal(expected.ToString(), solver.SolveFirst());
    }

    [Theory]
    [InlineData("R8, R4, R4, R8", 4)]
    public void TestSecondPart(string input, int expected)
    {
        var solver = new Day01();
        solver.Presolve(input);
        Assert.Equal(expected.ToString(), solver.SolveSecond());
    }
}