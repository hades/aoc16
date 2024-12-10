using aoc16;

namespace tests;

public class TestDay17 {
  [Theory]
  [InlineData("ihgpwlah", "DDRRRD")]
  [InlineData("kglvqrro", "DDUDRLRRUDRD")]
  [InlineData("ulqzkmiv", "DRURDRUDDLLDLUURRDULRLDUUDDDRR")]
  public void TestFirstPart(string input, string output) {
    var solver = new Day17();
    solver.Presolve(input);
    Assert.Equal(output, solver.SolveFirst());
  }

  [Theory]
  [InlineData("ihgpwlah", "370")]
  [InlineData("kglvqrro", "492")]
  [InlineData("ulqzkmiv", "830")]
  public void TestSecondPart(string input, string output) {
    var solver = new Day17();
    solver.Presolve(input);
    Assert.Equal(output, solver.SolveSecond());
  }
}