using aoc16;

namespace tests;

public class TestDay06
{
  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day06();
    solver.Presolve("eedadn\ndrvtee\neandsr\nraavrd\natevrs\ntsrnev\nsdttsa\nrasrtv\nnssdts\nntnada\nsvetve\ntesnvt\nvntsnd\nvrdear\ndvrsen\nenarar\n ");
    Assert.Equal("easter", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day06();
    solver.Presolve("eedadn\ndrvtee\neandsr\nraavrd\natevrs\ntsrnev\nsdttsa\nrasrtv\nnssdts\nntnada\nsvetve\ntesnvt\nvntsnd\nvrdear\ndvrsen\nenarar\n");
    Assert.Equal("advent", solver.SolveSecond());
  }
}