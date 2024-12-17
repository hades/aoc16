using aoc16;

namespace tests;

public class TestInterval {
  [Fact]
  public void TestBasics_EmptyInterval() {
    var interval = Interval<int>.Empty;
    Assert.True(interval.IsEmpty);
    Assert.Equal(0, interval.Lower);
    Assert.Equal(0, interval.Upper);
    Assert.Equal(EndSpecifier.Open, interval.Left);
    Assert.Equal(EndSpecifier.Open, interval.Right);
    Assert.False(interval.Contains(-1));
    Assert.False(interval.Contains(0));
    Assert.False(interval.Contains(1));
  }

  [Fact]
  public void TestBasics_LessThan() {
    var interval = Interval<int>.LessThan(50);
    Assert.False(interval.IsEmpty);
    Assert.Equal(0, interval.Lower);
    Assert.Equal(50, interval.Upper);
    Assert.Equal(EndSpecifier.Infinity, interval.Left);
    Assert.Equal(EndSpecifier.Open, interval.Right);
    Assert.True(interval.Contains(-50));
    Assert.True(interval.Contains(-1));
    Assert.True(interval.Contains(0));
    Assert.True(interval.Contains(49));
    Assert.False(interval.Contains(50));
    Assert.False(interval.Contains(51));
  }

  [Fact]
  public void TestBasics_LessOrEquals() {
    var interval = Interval<int>.LessThanOrEquals(50);
    Assert.False(interval.IsEmpty);
    Assert.Equal(0, interval.Lower);
    Assert.Equal(50, interval.Upper);
    Assert.Equal(EndSpecifier.Infinity, interval.Left);
    Assert.Equal(EndSpecifier.Closed, interval.Right);
    Assert.True(interval.Contains(-50));
    Assert.True(interval.Contains(-1));
    Assert.True(interval.Contains(0));
    Assert.True(interval.Contains(49));
    Assert.True(interval.Contains(50));
    Assert.False(interval.Contains(51));
  }

  [Fact]
  public void TestBasics_GreaterThan() {
    var interval = Interval<int>.GreaterThan(50);
    Assert.False(interval.IsEmpty);
    Assert.Equal(50, interval.Lower);
    Assert.Equal(0, interval.Upper);
    Assert.Equal(EndSpecifier.Open, interval.Left);
    Assert.Equal(EndSpecifier.Infinity, interval.Right);
    Assert.False(interval.Contains(-50));
    Assert.False(interval.Contains(-1));
    Assert.False(interval.Contains(0));
    Assert.False(interval.Contains(49));
    Assert.False(interval.Contains(50));
    Assert.True(interval.Contains(51));
  }

  [Fact]
  public void TestBasics_GreaterOrEquals() {
    var interval = Interval<int>.GreaterThanOrEquals(50);
    Assert.False(interval.IsEmpty);
    Assert.Equal(50, interval.Lower);
    Assert.Equal(0, interval.Upper);
    Assert.Equal(EndSpecifier.Closed, interval.Left);
    Assert.Equal(EndSpecifier.Infinity, interval.Right);
    Assert.False(interval.Contains(-50));
    Assert.False(interval.Contains(-1));
    Assert.False(interval.Contains(0));
    Assert.False(interval.Contains(49));
    Assert.True(interval.Contains(50));
    Assert.True(interval.Contains(51));
  }

  [Fact]
  public void TestBasics_Open() {
    var interval = Interval<int>.Open(-50, 50);
    Assert.False(interval.IsEmpty);
    Assert.Equal(-50, interval.Lower);
    Assert.Equal(50, interval.Upper);
    Assert.Equal(EndSpecifier.Open, interval.Left);
    Assert.Equal(EndSpecifier.Open, interval.Right);
    Assert.False(interval.Contains(-51));
    Assert.False(interval.Contains(-50));
    Assert.True(interval.Contains(-1));
    Assert.True(interval.Contains(0));
    Assert.True(interval.Contains(49));
    Assert.False(interval.Contains(50));
    Assert.False(interval.Contains(51));
  }

  [Fact]
  public void TestBasics_Closed() {
    var interval = Interval<int>.Closed(-50, 50);
    Assert.False(interval.IsEmpty);
    Assert.Equal(-50, interval.Lower);
    Assert.Equal(50, interval.Upper);
    Assert.Equal(EndSpecifier.Closed, interval.Left);
    Assert.Equal(EndSpecifier.Closed, interval.Right);
    Assert.False(interval.Contains(-51));
    Assert.True(interval.Contains(-50));
    Assert.True(interval.Contains(-1));
    Assert.True(interval.Contains(0));
    Assert.True(interval.Contains(49));
    Assert.True(interval.Contains(50));
    Assert.False(interval.Contains(51));
  }

  [Fact]
  public void TestBasics_OpenClosed() {
    var interval = Interval<int>.OpenClosed(-50, 50);
    Assert.False(interval.IsEmpty);
    Assert.Equal(-50, interval.Lower);
    Assert.Equal(50, interval.Upper);
    Assert.Equal(EndSpecifier.Open, interval.Left);
    Assert.Equal(EndSpecifier.Closed, interval.Right);
    Assert.False(interval.Contains(-51));
    Assert.False(interval.Contains(-50));
    Assert.True(interval.Contains(-1));
    Assert.True(interval.Contains(0));
    Assert.True(interval.Contains(49));
    Assert.True(interval.Contains(50));
    Assert.False(interval.Contains(51));
  }

  [Fact]
  public void TestBasics_ClosedOpen() {
    var interval = Interval<int>.ClosedOpen(-50, 50);
    Assert.False(interval.IsEmpty);
    Assert.Equal(-50, interval.Lower);
    Assert.Equal(50, interval.Upper);
    Assert.Equal(EndSpecifier.Closed, interval.Left);
    Assert.Equal(EndSpecifier.Open, interval.Right);
    Assert.False(interval.Contains(-51));
    Assert.True(interval.Contains(-50));
    Assert.True(interval.Contains(-1));
    Assert.True(interval.Contains(0));
    Assert.True(interval.Contains(49));
    Assert.False(interval.Contains(50));
    Assert.False(interval.Contains(51));
  }

  [Fact]
  public void TestBasics_Singleton() {
    var interval = Interval<int>.Singleton(50);
    Assert.False(interval.IsEmpty);
    Assert.Equal(50, interval.Lower);
    Assert.Equal(50, interval.Upper);
    Assert.Equal(EndSpecifier.Closed, interval.Left);
    Assert.Equal(EndSpecifier.Closed, interval.Right);
    Assert.False(interval.Contains(-51));
    Assert.False(interval.Contains(-50));
    Assert.False(interval.Contains(-1));
    Assert.False(interval.Contains(0));
    Assert.False(interval.Contains(49));
    Assert.True(interval.Contains(50));
    Assert.False(interval.Contains(51));
  }

  [Theory]
  [InlineData(0,
    0, 0, EndSpecifier.Infinity, EndSpecifier.Infinity,
    0, 0, EndSpecifier.Infinity, EndSpecifier.Infinity)]
  [InlineData(0,
    0, 1, EndSpecifier.Infinity, EndSpecifier.Open,
    0, 1, EndSpecifier.Infinity, EndSpecifier.Open)]
  [InlineData(0,
    0, 1, EndSpecifier.Infinity, EndSpecifier.Closed,
    0, 1, EndSpecifier.Infinity, EndSpecifier.Closed)]
  [InlineData(0,
    0, 0, EndSpecifier.Open, EndSpecifier.Infinity,
    0, 0, EndSpecifier.Open, EndSpecifier.Infinity)]
  [InlineData(0,
    0, 0, EndSpecifier.Closed, EndSpecifier.Infinity,
    0, 0, EndSpecifier.Closed, EndSpecifier.Infinity)]
  [InlineData(0,
    0, 1, EndSpecifier.Infinity, EndSpecifier.Closed,
    0, 1, EndSpecifier.Infinity, EndSpecifier.Open)]
  [InlineData(-1,
    -1, 0, EndSpecifier.Closed, EndSpecifier.Infinity,
    -1, 0, EndSpecifier.Open, EndSpecifier.Infinity)]
  [InlineData(0,
    0, 5, EndSpecifier.Open, EndSpecifier.Open,
    0, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(0,
    0, 5, EndSpecifier.Open, EndSpecifier.Closed,
    0, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(0,
    0, 5, EndSpecifier.Open, EndSpecifier.Closed,
    0, 5, EndSpecifier.Open, EndSpecifier.Closed)]
  [InlineData(-1,
    0, 5, EndSpecifier.Closed, EndSpecifier.Open,
    0, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(-1,
    0, 5, EndSpecifier.Closed, EndSpecifier.Closed,
    0, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(-1,
    0, 5, EndSpecifier.Closed, EndSpecifier.Closed,
    0, 5, EndSpecifier.Open, EndSpecifier.Closed)]
  [InlineData(0,
    0, 5, EndSpecifier.Closed, EndSpecifier.Open,
    0, 5, EndSpecifier.Closed, EndSpecifier.Open)]
  [InlineData(0,
    0, 5, EndSpecifier.Closed, EndSpecifier.Closed,
    0, 5, EndSpecifier.Closed, EndSpecifier.Open)]
  [InlineData(0,
    0, 5, EndSpecifier.Closed, EndSpecifier.Closed,
    0, 5, EndSpecifier.Closed, EndSpecifier.Closed)]
  public void TestCompareTo_SameRange(int expected,
    int alower, int aupper, EndSpecifier aleft, EndSpecifier aright,
    int blower, int bupper, EndSpecifier bleft, EndSpecifier bright) {
    var a = new Interval<int>(alower, aupper, aleft, aright);
    var b = new Interval<int>(blower, bupper, bleft, bright);
    Assert.Equal(expected, a.CompareTo(b));
    Assert.Equal(expected, -b.CompareTo(a));
  }

  [Theory]
  [InlineData(0,
    0, 1, EndSpecifier.Infinity, EndSpecifier.Open,
    0, 5, EndSpecifier.Infinity, EndSpecifier.Open)]
  [InlineData(0,
    0, 1, EndSpecifier.Infinity, EndSpecifier.Closed,
    0, 5, EndSpecifier.Infinity, EndSpecifier.Closed)]
  [InlineData(0,
    0, 1, EndSpecifier.Infinity, EndSpecifier.Closed,
    0, 5, EndSpecifier.Infinity, EndSpecifier.Open)]
  [InlineData(-1,
    -1, 0, EndSpecifier.Closed, EndSpecifier.Infinity,
    -1, 0, EndSpecifier.Open, EndSpecifier.Infinity)]
  [InlineData(0,
    0, 1, EndSpecifier.Open, EndSpecifier.Open,
    0, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(0,
    0, 1, EndSpecifier.Open, EndSpecifier.Closed,
    0, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(0,
    0, 1, EndSpecifier.Open, EndSpecifier.Closed,
    0, 5, EndSpecifier.Open, EndSpecifier.Closed)]
  [InlineData(-1,
    0, 1, EndSpecifier.Closed, EndSpecifier.Open,
    0, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(-1,
    0, 1, EndSpecifier.Closed, EndSpecifier.Closed,
    0, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(-1,
    0, 1, EndSpecifier.Closed, EndSpecifier.Closed,
    0, 5, EndSpecifier.Open, EndSpecifier.Closed)]
  [InlineData(0,
    0, 1, EndSpecifier.Closed, EndSpecifier.Open,
    0, 5, EndSpecifier.Closed, EndSpecifier.Open)]
  [InlineData(0,
    0, 1, EndSpecifier.Closed, EndSpecifier.Closed,
    0, 5, EndSpecifier.Closed, EndSpecifier.Open)]
  [InlineData(0,
    0, 1, EndSpecifier.Closed, EndSpecifier.Closed,
    0, 5, EndSpecifier.Closed, EndSpecifier.Closed)]
  public void TestCompareTo_SameLeft(int expected,
    int alower, int aupper, EndSpecifier aleft, EndSpecifier aright,
    int blower, int bupper, EndSpecifier bleft, EndSpecifier bright) {
    var a = new Interval<int>(alower, aupper, aleft, aright);
    var b = new Interval<int>(blower, bupper, bleft, bright);
    Assert.Equal(expected, a.CompareTo(b));
    Assert.Equal(expected, -b.CompareTo(a));
  }

  [Theory]
  [InlineData(-1,
    0, 4, EndSpecifier.Open, EndSpecifier.Open,
    1, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(-1,
    0, 4, EndSpecifier.Open, EndSpecifier.Closed,
    1, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(-1,
    0, 4, EndSpecifier.Open, EndSpecifier.Closed,
    1, 5, EndSpecifier.Open, EndSpecifier.Closed)]
  [InlineData(-1,
    0, 4, EndSpecifier.Closed, EndSpecifier.Open,
    1, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(-1,
    0, 4, EndSpecifier.Closed, EndSpecifier.Closed,
    1, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(-1,
    0, 4, EndSpecifier.Closed, EndSpecifier.Closed,
    1, 5, EndSpecifier.Open, EndSpecifier.Closed)]
  [InlineData(-1,
    0, 4, EndSpecifier.Closed, EndSpecifier.Open,
    1, 5, EndSpecifier.Closed, EndSpecifier.Open)]
  [InlineData(-1,
    0, 4, EndSpecifier.Closed, EndSpecifier.Closed,
    1, 5, EndSpecifier.Closed, EndSpecifier.Open)]
  [InlineData(-1,
    0, 4, EndSpecifier.Closed, EndSpecifier.Closed,
    1, 5, EndSpecifier.Closed, EndSpecifier.Closed)]
  public void TestCompareTo_Overlapping(int expected,
    int alower, int aupper, EndSpecifier aleft, EndSpecifier aright,
    int blower, int bupper, EndSpecifier bleft, EndSpecifier bright) {
    var a = new Interval<int>(alower, aupper, aleft, aright);
    var b = new Interval<int>(blower, bupper, bleft, bright);
    Assert.Equal(expected, a.CompareTo(b));
    Assert.Equal(expected, -b.CompareTo(a));
  }

  private static readonly Interval<int> empty = Interval<int>.Empty;
  private static readonly Interval<int> everything = new(default, default, EndSpecifier.Infinity, EndSpecifier.Infinity);
  private static readonly Interval<int> less_than = Interval<int>.LessThan(50);
  private static readonly Interval<int> greater_than = Interval<int>.GreaterThan(50);
  private static readonly Interval<int> less_or_equals = Interval<int>.LessThanOrEquals(50);
  private static readonly Interval<int> greater_or_equals = Interval<int>.GreaterThanOrEquals(50);
  private static readonly Interval<int> singleton = Interval<int>.Singleton(50);

  [Fact]
  public void TestInvert_Basic() {
    Assert.Equal(empty, ~everything);
    Assert.Equal(less_than, ~greater_or_equals);
    Assert.Equal(greater_than, ~less_or_equals);
    Assert.Equal(everything, ~empty);
    Assert.Equal(greater_or_equals, ~less_than);
    Assert.Equal(less_or_equals, ~greater_than);
    Assert.True((~empty).Contains(49));
    Assert.True((~empty).Contains(50));
    Assert.True((~empty).Contains(51));
    Assert.False((~less_than).Contains(49));
    Assert.True((~less_than).Contains(50));
    Assert.True((~less_than).Contains(51));
    Assert.True((~greater_than).Contains(49));
    Assert.True((~greater_than).Contains(50));
    Assert.False((~greater_than).Contains(51));
    Assert.False((~everything).Contains(49));
    Assert.False((~everything).Contains(50));
    Assert.False((~everything).Contains(51));
    Assert.False((~less_or_equals).Contains(49));
    Assert.False((~less_or_equals).Contains(50));
    Assert.True((~less_or_equals).Contains(51));
    Assert.True((~greater_or_equals).Contains(49));
    Assert.False((~greater_or_equals).Contains(50));
    Assert.False((~greater_or_equals).Contains(51));
    Assert.True((~singleton).Contains(49));
    Assert.False((~singleton).Contains(50));
    Assert.True((~singleton).Contains(51));
  }

  [Fact]
  public void TestUnion_Basic() {
    Assert.Equal(empty, empty | empty);
    Assert.Equal(everything, empty | everything);
    Assert.Equal(less_than, empty | less_than);
    Assert.Equal(greater_than, empty | greater_than);
    Assert.Equal(less_or_equals, empty | less_or_equals);
    Assert.Equal(greater_or_equals, empty | greater_or_equals);
    Assert.Equal(singleton, empty | singleton);

    Assert.Equal(everything, everything | empty);
    Assert.Equal(everything, everything | everything);
    Assert.Equal(everything, everything | less_than);
    Assert.Equal(everything, everything | greater_than);
    Assert.Equal(everything, everything | less_or_equals);
    Assert.Equal(everything, everything | greater_or_equals);
    Assert.Equal(everything, everything | singleton);

    Assert.Equal(less_than, less_than | empty);
    Assert.Equal(everything, less_than | everything);
    Assert.Equal(less_than, less_than | less_than);
    Assert.Equal((~singleton), less_than | greater_than);
    Assert.Equal(less_or_equals, less_than | less_or_equals);
    Assert.Equal(everything, less_than | greater_or_equals);
    Assert.Equal(less_or_equals, less_than | singleton);

    Assert.Equal(greater_than, greater_than | empty);
    Assert.Equal(everything, greater_than | everything);
    Assert.Equal((~singleton), greater_than | less_than);
    Assert.Equal(greater_than, greater_than | greater_than);
    Assert.Equal(everything, greater_than | less_or_equals);
    Assert.Equal(greater_or_equals, greater_than | greater_or_equals);
    Assert.Equal(greater_or_equals, greater_than | singleton);

    Assert.Equal(less_or_equals, less_or_equals | empty);
    Assert.Equal(everything, less_or_equals | everything);
    Assert.Equal(less_or_equals, less_or_equals | less_than);
    Assert.Equal(everything, less_or_equals | greater_than);
    Assert.Equal(less_or_equals, less_or_equals | less_or_equals);
    Assert.Equal(everything, less_or_equals | greater_or_equals);
    Assert.Equal(less_or_equals, less_or_equals | singleton);

    Assert.Equal(greater_or_equals, greater_or_equals | empty);
    Assert.Equal(everything, greater_or_equals | everything);
    Assert.Equal(everything, greater_or_equals | less_than);
    Assert.Equal(greater_or_equals, greater_or_equals | greater_than);
    Assert.Equal(everything, greater_or_equals | less_or_equals);
    Assert.Equal(greater_or_equals, greater_or_equals | greater_or_equals);
    Assert.Equal(greater_or_equals, greater_or_equals | singleton);

    Assert.Equal(singleton, singleton | empty);
    Assert.Equal(everything, singleton | everything);
    Assert.Equal(less_or_equals, singleton | less_than);
    Assert.Equal(greater_or_equals, singleton | greater_than);
    Assert.Equal(less_or_equals, singleton | less_or_equals);
    Assert.Equal(greater_or_equals, singleton | greater_or_equals);
    Assert.Equal(singleton, singleton | singleton);
  }

  [Fact]
  public void TestUnion_TwoSegments() {
    var interval = Interval<long>.Closed(1, 5)
      | Interval<long>.Closed(10, 15);
    Assert.False(interval.Contains(-50));
    Assert.False(interval.Contains(0));
    Assert.True(interval.Contains(1));
    Assert.True(interval.Contains(2));
    Assert.True(interval.Contains(3));
    Assert.True(interval.Contains(4));
    Assert.True(interval.Contains(5));
    Assert.False(interval.Contains(6));
    Assert.False(interval.Contains(9));
    Assert.True(interval.Contains(10));
    Assert.True(interval.Contains(11));
    Assert.True(interval.Contains(12));
    Assert.True(interval.Contains(13));
    Assert.True(interval.Contains(14));
    Assert.True(interval.Contains(15));
    Assert.False(interval.Contains(16));
    Assert.False(interval.Contains(50));
  }

  [Fact]
  public void TestInvert_TwoSegments() {
    var interval = ~(Interval<long>.Closed(1, 5)
      | Interval<long>.Closed(10, 15));
    Assert.True(interval.Contains(-50));
    Assert.True(interval.Contains(0));
    Assert.False(interval.Contains(1));
    Assert.False(interval.Contains(2));
    Assert.False(interval.Contains(3));
    Assert.False(interval.Contains(4));
    Assert.False(interval.Contains(5));
    Assert.True(interval.Contains(6));
    Assert.True(interval.Contains(9));
    Assert.False(interval.Contains(10));
    Assert.False(interval.Contains(11));
    Assert.False(interval.Contains(12));
    Assert.False(interval.Contains(13));
    Assert.False(interval.Contains(14));
    Assert.False(interval.Contains(15));
    Assert.True(interval.Contains(16));
    Assert.True(interval.Contains(50));
  }

  [Fact]
  public void TestUnion_ThreeSegments() {
    var interval = Interval<long>.Closed(2365712272, 2390766206)
      | Interval<long>.Closed(2569947483, 2579668543)
      | Interval<long>.Closed(1348241901, 1362475328);
    Assert.False(interval.Contains(2365712271));
    Assert.True(interval.Contains(2365712272));
    Assert.True(interval.Contains(2365712273));
    Assert.True(interval.Contains(2390766205));
    Assert.True(interval.Contains(2390766206));
    Assert.False(interval.Contains(2390766207));
    Assert.False(interval.Contains(2569947482));
    Assert.True(interval.Contains(2569947483));
    Assert.True(interval.Contains(2569947484));
    Assert.True(interval.Contains(2579668542));
    Assert.True(interval.Contains(2579668543));
    Assert.False(interval.Contains(2579668544));
    Assert.False(interval.Contains(1348241900));
    Assert.True(interval.Contains(1348241901));
    Assert.True(interval.Contains(1348241902));
    Assert.True(interval.Contains(1362475327));
    Assert.True(interval.Contains(1362475328));
    Assert.False(interval.Contains(1362475329));
  }

  [Fact]
  public void TestInvert_ThreeSegments() {
    var interval = ~(Interval<long>.Closed(2365712272, 2390766206)
      | Interval<long>.Closed(2569947483, 2579668543)
      | Interval<long>.Closed(1348241901, 1362475328));
    Assert.True(interval.Contains(2365712271));
    Assert.False(interval.Contains(2365712272));
    Assert.False(interval.Contains(2365712273));
    Assert.False(interval.Contains(2390766205));
    Assert.False(interval.Contains(2390766206));
    Assert.True(interval.Contains(2390766207));
    Assert.True(interval.Contains(2569947482));
    Assert.False(interval.Contains(2569947483));
    Assert.False(interval.Contains(2569947484));
    Assert.False(interval.Contains(2579668542));
    Assert.False(interval.Contains(2579668543));
    Assert.True(interval.Contains(2579668544));
    Assert.True(interval.Contains(1348241900));
    Assert.False(interval.Contains(1348241901));
    Assert.False(interval.Contains(1348241902));
    Assert.False(interval.Contains(1362475327));
    Assert.False(interval.Contains(1362475328));
    Assert.True(interval.Contains(1362475329));
  }

  [Theory]
  [InlineData(
    0, 5, EndSpecifier.Open, EndSpecifier.Open,
    0, 4, EndSpecifier.Open, EndSpecifier.Open,
    1, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(
    0, 5, EndSpecifier.Closed, EndSpecifier.Open,
    0, 4, EndSpecifier.Closed, EndSpecifier.Open,
    1, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(
    0, 5, EndSpecifier.Open, EndSpecifier.Open,
    0, 4, EndSpecifier.Open, EndSpecifier.Closed,
    1, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(
    0, 5, EndSpecifier.Open, EndSpecifier.Open,
    0, 4, EndSpecifier.Open, EndSpecifier.Open,
    1, 5, EndSpecifier.Closed, EndSpecifier.Open)]
  [InlineData(
    0, 5, EndSpecifier.Open, EndSpecifier.Closed,
    0, 4, EndSpecifier.Open, EndSpecifier.Open,
    1, 5, EndSpecifier.Open, EndSpecifier.Closed)]
  [InlineData(
    0, 5, EndSpecifier.Closed, EndSpecifier.Closed,
    0, 4, EndSpecifier.Closed, EndSpecifier.Open,
    1, 5, EndSpecifier.Open, EndSpecifier.Closed)]
  public void TestUnion_TwoSegments_Overlapping(
    int rlower, int rupper, EndSpecifier rleft, EndSpecifier rright,
    int alower, int aupper, EndSpecifier aleft, EndSpecifier aright,
    int blower, int bupper, EndSpecifier bleft, EndSpecifier bright) {
    var a = new Interval<int>(alower, aupper, aleft, aright);
    var b = new Interval<int>(blower, bupper, bleft, bright);
    var r = new Interval<int>(rlower, rupper, rleft, rright);
    Assert.Equal(r, a | b);
    Assert.Equal(r, b | a);
  }

  [Theory]
  [InlineData(
    0, 5, EndSpecifier.Open, EndSpecifier.Open,
    0, 4, EndSpecifier.Open, EndSpecifier.Closed,
    4, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(
    0, 5, EndSpecifier.Open, EndSpecifier.Open,
    0, 4, EndSpecifier.Open, EndSpecifier.Open,
    4, 5, EndSpecifier.Closed, EndSpecifier.Open)]
  public void TestUnion_TwoSegments_Adjacent(
    int rlower, int rupper, EndSpecifier rleft, EndSpecifier rright,
    int alower, int aupper, EndSpecifier aleft, EndSpecifier aright,
    int blower, int bupper, EndSpecifier bleft, EndSpecifier bright) {
    var a = new Interval<int>(alower, aupper, aleft, aright);
    var b = new Interval<int>(blower, bupper, bleft, bright);
    var r = new Interval<int>(rlower, rupper, rleft, rright);
    Assert.Equal(r, a | b);
    Assert.Equal(r, b | a);
  }

  [Theory]
  [InlineData(
    0, 5, EndSpecifier.Infinity, EndSpecifier.Open,
    0, 4, EndSpecifier.Infinity, EndSpecifier.Closed,
    4, 5, EndSpecifier.Open, EndSpecifier.Open)]
  [InlineData(
    0, 0, EndSpecifier.Open, EndSpecifier.Infinity,
    0, 4, EndSpecifier.Open, EndSpecifier.Open,
    4, 0, EndSpecifier.Closed, EndSpecifier.Infinity)]
  public void TestUnion_TwoSegments_Infinity(
    int rlower, int rupper, EndSpecifier rleft, EndSpecifier rright,
    int alower, int aupper, EndSpecifier aleft, EndSpecifier aright,
    int blower, int bupper, EndSpecifier bleft, EndSpecifier bright) {
    var a = new Interval<int>(alower, aupper, aleft, aright);
    var b = new Interval<int>(blower, bupper, bleft, bright);
    var r = new Interval<int>(rlower, rupper, rleft, rright);
    Assert.Equal(r, a | b);
    Assert.Equal(r, b | a);
  }
}
