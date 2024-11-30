using aoc16;

namespace tests;

public class TestDay04
{
  [Fact]
  public void TestFirstPart()
  {
    var solver = new Day04();
    solver.Presolve("a-b-c-d-e-f-g-h-987[abcde]\naaaaa-bbb-z-y-x-123[abxyz]\nnot-a-real-room-404[oarel]\ntotally-real-room-200[decoy]");
    Assert.Equal("1514", solver.SolveFirst());
  }

  [Fact]
  public void TestSecondPart()
  {
    var solver = new Day04();
    solver.Presolve("a-b-c-d-e-f-g-h-987[abcde]\naaaaa-bbb-z-y-x-123[abxyz]\nnot-a-real-room-404[oarel]\nnorthpole-object-storage-26[whatever]\ntotally-real-room-200[decoy]");
    Assert.Equal("26", solver.SolveSecond());
  }
}