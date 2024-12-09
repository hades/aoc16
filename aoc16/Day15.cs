using System.Numerics;
using System.Text.RegularExpressions;

namespace aoc16;

[ForDay(15)]
public partial class Day15 : Solver {
  [GeneratedRegex(@"Disc #(\d+) has (\d+) positions; at time=0, it is at position (\d+).")]
  private static partial Regex DiscSpecRe();

  private List<(int, int)> discs = [];

  public void Presolve(string input) {
    discs = input.Trim().Split("\n").Select(line => {
      var match = DiscSpecRe().Match(line);
      return (int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
    }).ToList();
  }

  private (BigInteger gcd, List<BigInteger> bézout) ExtendedEuclideanAlgorithm(List<BigInteger> vals) {
    var g = vals[0];
    List<BigInteger> cs = [1];
    for (int i = 1; i < vals.Count; i++) {
      var s = g;
      var t = vals[i];
      BigInteger s0 = 1;
      BigInteger s1 = 0;
      BigInteger t0 = 0;
      BigInteger t1 = 1;
      while (t != 0) {
        var (quotient, remainder) = BigInteger.DivRem(s, t);
        s = t;
        t = remainder;
        (s0, s1) = (s1, s0 - quotient * s1);
        (t0, t1) = (t1, t0 - quotient * t1);
      }
      (g, s, t) = (s, s0, t0);
      cs = cs.Select(c => c * s).Union([t]).ToList();
    }
    return g > 0 ? (g, cs) : (-g, cs.Select(c => -c).ToList());
  }

  public string Solve(List<(int, int)> discs) {
    var eucd = ExtendedEuclideanAlgorithm([discs[0].Item1, discs[1].Item1]);
    BigInteger factor = discs[0].Item1 * discs[1].Item1;
    var cur_solution = ((-1 - discs[0].Item2) * discs[1].Item1 * eucd.bézout[1] +
                        (-2 - discs[1].Item2) * discs[0].Item1 * eucd.bézout[0]) % factor;
    for (int i = 2; i < discs.Count; i++) {
      eucd = ExtendedEuclideanAlgorithm([factor, discs[i].Item1]);
      cur_solution = (cur_solution * discs[i].Item1 * eucd.bézout[1] +
                      (-i - 1 - discs[i].Item2) * factor * eucd.bézout[0]);
      factor *= discs[i].Item1;
      cur_solution %= factor;
    }
    return ((cur_solution + factor) % factor).ToString();
  }

  public string SolveFirst() => Solve(discs);
  public string SolveSecond() => Solve(discs.Union([(11, 0)]).ToList());
}