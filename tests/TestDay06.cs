using aoc16;

namespace tests;

public class TestDay06
{
  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day06();
    solver.Presolve(@"eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar
");
    Assert.Equal("easter", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day06();
    solver.Presolve(@"eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar
");
    Assert.Equal("advent", solver.SolveSecond());
  }
}