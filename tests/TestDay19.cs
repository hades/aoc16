using aoc16;

namespace tests;

public class TestDay19 {
  [Theory]
  [InlineData("1", "1")]
  [InlineData("2", "1")]
  [InlineData("3", "3")]
  [InlineData("4", "1")]
  [InlineData("5", "3")]
  [InlineData("6", "5")]
  [InlineData("7", "7")]
  [InlineData("8", "1")]
  [InlineData("9", "3")]
  [InlineData("10", "5")]
  [InlineData("11", "7")]
  [InlineData("12", "9")]
  [InlineData("13", "11")]
  [InlineData("14", "13")]
  [InlineData("15", "15")]
  [InlineData("16", "1")]
  [InlineData("17", "3")]
  [InlineData("18", "5")]
  [InlineData("19", "7")]
  [InlineData("20", "9")]
  [InlineData("21", "11")]
  [InlineData("22", "13")]
  [InlineData("23", "15")]
  [InlineData("24", "17")]
  [InlineData("25", "19")]
  [InlineData("26", "21")]
  [InlineData("27", "23")]
  [InlineData("3014387", "1834471")]
  public void TestFirstPart(string input, string output) {
    var solver = new Day19();
    solver.Presolve(input);
    Assert.Equal(output, solver.SolveFirst());
  }

  [Theory]
  [InlineData("1", "1")]
  [InlineData("2", "1")]
  [InlineData("3", "3")]
  [InlineData("4", "1")]
  [InlineData("5", "2")]
  [InlineData("6", "3")]
  [InlineData("7", "5")]
  [InlineData("8", "7")]
  [InlineData("9", "9")]
  [InlineData("10", "1")]
  [InlineData("11", "2")]
  [InlineData("12", "3")]
  [InlineData("13", "4")]
  [InlineData("14", "5")]
  [InlineData("15", "6")]
  [InlineData("16", "7")]
  [InlineData("17", "8")]
  [InlineData("18", "9")]
  [InlineData("19", "11")]
  [InlineData("20", "13")]
  [InlineData("21", "15")]
  [InlineData("22", "17")]
  [InlineData("23", "19")]
  [InlineData("24", "21")]
  [InlineData("25", "23")]
  [InlineData("26", "25")]
  [InlineData("27", "27")]
  [InlineData("3014387", "1420064")]
  public void TestSecondPart(string input, string output) {
    var solver = new Day19();
    solver.Presolve(input);
    Assert.Equal(output, solver.SolveSecond());
  }
}