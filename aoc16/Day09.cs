using System.Text;
using System.Text.RegularExpressions;

namespace aoc16;

[ForDay(9)]
public partial class Day09 : Solver
{
  private string input = "";

  public void Presolve(string input)
  {
    this.input = string.Join("", input.Trim().Split('\n'));
  }

  public string SolveFirst()
  {
    var remaining = input;
    var output = "";
    while (true)
    {
      var m = CompresionMarkerRegex().Match(remaining);
      if (!m.Success)
      {
        output += remaining;
        break;
      }

      output += remaining[..m.Index];
      var (length, repeat) = (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
      output += new StringBuilder()
        .Insert(0, remaining[(m.Index + m.Length)..(m.Index + m.Length + length)], repeat)
        .ToString();
      remaining = remaining[(m.Index + m.Length + length)..];
    }

    return output.Length.ToString();
  }

  public string SolveSecond()
  {
    return DecompressedLength(input).ToString();
  }

  [GeneratedRegex(@"\((\d+)x(\d+)\)")]
  private static partial Regex CompresionMarkerRegex();

  private static long DecompressedLength(string input)
  {
    var remaining = input;
    long result = 0;
    while (true)
    {
      var m = CompresionMarkerRegex().Match(remaining);
      if (!m.Success)
      {
        result += remaining.Length;
        break;
      }

      result += m.Index;
      var (length, repeat) = (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
      result += repeat *
                DecompressedLength(remaining[(m.Index + m.Length)..(m.Index + m.Length + length)]);
      remaining = remaining[(m.Index + m.Length + length)..];
    }

    return result;
  }
}