using System.Collections.Immutable;

namespace aoc16;

[ForDay(2)]
public class Day02 : Solver
{
  private static readonly ImmutableList<string> ACTUAL_KEYBOARD =
  [
    "  1  ",
    " 234 ",
    "56789",
    " ABC ",
    "  D  "
  ];

  private ImmutableList<string> directions = [];

  public void Presolve(string input)
  {
    directions = [.. input.Trim().Split("\n")];
  }

  public string SolveFirst()
  {
    var currentDigit = 5;
    var code = "";
    foreach (var line in directions)
    {
      foreach (var c in line)
      {
        if (c == 'U' && currentDigit > 3) currentDigit -= 3;
        if (c == 'D' && currentDigit < 7) currentDigit += 3;
        if (c == 'L' && currentDigit % 3 != 1) currentDigit--;
        if (c == 'R' && currentDigit % 3 != 0) currentDigit++;
      }

      code += currentDigit.ToString();
    }

    return code;
  }

  public string SolveSecond()
  {
    int i = 2, j = 0;
    var code = "";
    foreach (var line in directions)
    {
      foreach (var c in line)
      {
        int newI = i, newJ = j;
        if (c == 'U') newI--;
        if (c == 'D') newI++;
        if (c == 'L') newJ--;
        if (c == 'R') newJ++;
        if (newI >= 0 && newI < ACTUAL_KEYBOARD.Count && newJ >= 0 &&
            newJ < ACTUAL_KEYBOARD[newI].Length && ACTUAL_KEYBOARD[newI][newJ] != ' ')
        {
          i = newI;
          j = newJ;
        }
      }

      code += ACTUAL_KEYBOARD[i][j];
    }

    return code;
  }
}