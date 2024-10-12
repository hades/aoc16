using aoc16;

namespace tests;

public class TestDay07
{
  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day07();
    solver.Presolve(@"abba[mnop]qrst
abcd[bddb]xyyx
aaaa[qwer]tyui
ioxxoj[asdfgh]zxcvbn
ioxxoj[asdfgh]zxcvbn[abba]xyz
");
    Assert.Equal("2", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day07();
    solver.Presolve(@"aba[bab]xyz
xyx[xyx]xyx
aaa[kek]eke
zazbz[bzb]cdb");
    Assert.Equal("3", solver.SolveSecond());
  }
}