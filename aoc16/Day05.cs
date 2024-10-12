using System.Security.Cryptography;
using System.Text;

namespace aoc16;

[ForDay(5)]
public class Day05 : Solver
{
  private static readonly Random rand = new();
  private string input = "";

  public void Presolve(string input)
  {
    this.input = input.Trim();
  }

  public string SolveFirst()
  {
    var password = "";
    var suffix = 0;
    while (password.Length < 8)
    {
      var hash = MD5.HashData(Encoding.UTF8.GetBytes($"{input}{suffix}"));
      if (hash[0] == 0 && hash[1] == 0 && hash[2] < 0x10) password += hash[2].ToString("x");
      suffix += 1;
    }

    return password.ToLower();
  }

  public string SolveSecond()
  {
    var password = new char[8];
    var suffix = 0;
    Console.WriteLine();
    Console.CursorVisible = false;
    while (Array.IndexOf(password, (char)0) >= 0)
    {
      var hash = MD5.HashData(Encoding.UTF8.GetBytes($"{input}{suffix}"));
      if (hash[0] == 0 && hash[1] == 0 && hash[2] < 0x10)
      {
        var position = hash[2] & 0x0f;
        if (position < 8 && password[position] == 0)
          password[position] = (hash[3] >> 4).ToString("x").ToLower()[0];
      }

      suffix += 1;
      if (suffix % 100 == 0) CoolHackyAnimation(password);
    }

    var passwordStr = new string(password);
    Console.WriteLine("\r" + passwordStr);
    Console.CursorVisible = true;
    return passwordStr;
  }

  private void CoolHackyAnimation(char[] password)
  {
    Console.Write("\r");
    for (var i = 0; i < 8; ++i)
      Console.Write(password[i] == 0 ? rand.Next(16).ToString("x") : password[i]);
  }
}