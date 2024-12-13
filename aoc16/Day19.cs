using System.Numerics;

namespace aoc16;

[ForDay(19)]
public partial class Day19 : Solver {
  private Int32 number_of_elves;

  public void Presolve(string input) {
    number_of_elves = int.Parse(input.Trim());
  }

  public string SolveFirst() {
    int mask = 0x40000000 >> (BitOperations.LeadingZeroCount((uint)number_of_elves) - 2);
    return (((number_of_elves << 1) & ~mask) | 1).ToString();
  }

  public string SolveSecond() {
    int elog3 = 1;
    while (elog3 < number_of_elves) {
      elog3 *= 3;
    }
    if (elog3 == number_of_elves) {
      return number_of_elves.ToString();
    }
    elog3 /= 3;
    if (number_of_elves < 2*elog3) {
      return (number_of_elves % elog3).ToString();
    }
    return (2 * (number_of_elves % elog3) + elog3).ToString();
  }
}