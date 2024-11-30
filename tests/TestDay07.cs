using aoc16;

namespace tests;

public class TestDay07
{
  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day07();
    solver.Presolve("abba[mnop]qrst\nabcd[bddb]xyyx\naaaa[qwer]tyui\nioxxoj[asdfgh]zxcvbn\nioxxoj[asdfgh]zxcvbn[abba]xyz");
    Assert.Equal("2", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day07();
    solver.Presolve("aba[bab]xyz\nxyx[xyx]xyx\naaa[kek]eke\nzazbz[bzb]cdb");
    Assert.Equal("3", solver.SolveSecond());
  }
}