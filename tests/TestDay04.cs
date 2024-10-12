using aoc16;

namespace tests;

public class TestDay04
{
  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day04();
    solver.Presolve(@"a-b-c-d-e-f-g-h-987[abcde]
aaaaa-bbb-z-y-x-123[abxyz]
not-a-real-room-404[oarel]
totally-real-room-200[decoy]");
    Assert.Equal("1514", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day04();
    solver.Presolve(@"a-b-c-d-e-f-g-h-987[abcde]
aaaaa-bbb-z-y-x-123[abxyz]
not-a-real-room-404[oarel]
northpole-object-storage-26[whatever]
totally-real-room-200[decoy]");
    Assert.Equal("26", solver.SolveSecond());
  }
}