using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace aoc16;

[ForDay(12)]
public partial class Day12 : Solver {
  private enum Register { A, B, C, D };

  private static Register RegisterFromString(string s) {
    if (!Enum.TryParse<Register>(s.ToUpper(), out var r)) {
      throw new InvalidDataException($"{s} is not a register");
    }
    return r;
  }

  private abstract record ImmediateOrRegisterOperand;
  private sealed record ImmediateOperand(int Value) : ImmediateOrRegisterOperand;
  private sealed record RegisterOperand(Register Register) : ImmediateOrRegisterOperand;

  private abstract record Command;
  private sealed record Cpy(
    ImmediateOrRegisterOperand From,
    Register To
    ) : Command;
  private sealed record Inc(Register Register) : Command;
  private sealed record Dec(Register Register) : Command;
  private sealed record Jnz(Register Register, int Offset) : Command;

  [GeneratedRegex(@"[abcd]")]
  private static partial Regex RegisterRe();

  private List<Command> commands = [];

  public void Presolve(string input) {
    commands = input.Trim().Split('\n').Select<string, Command>(line => {
      var tokens = line.Split(" ");
      return tokens[0] switch {
        "cpy" => new Cpy(
          RegisterRe().Match(tokens[1]) switch {
            { Success: true } match => new RegisterOperand(RegisterFromString(match.Value)),
            _ => new ImmediateOperand(int.Parse(tokens[1])),
          },
          RegisterFromString(tokens[2])
          ),
        "inc" => new Inc(RegisterFromString(tokens[1])),
        "dec" => new Dec(RegisterFromString(tokens[1])),
        "jnz" => new Jnz(RegisterFromString(tokens[1]), int.Parse(tokens[2])),
        _ => throw new InvalidDataException($"could not parse command: {line}"),
      };
    }).ToList();
  }

  private string Solve(Dictionary<Register, int> registers) {
    int ip = 0;
    while (ip >= 0 && ip < commands.Count) {
      int next_ip = ip + 1;
      switch (commands[ip]) {
        case Cpy cpy:
          registers[cpy.To] = cpy.From switch {
            ImmediateOperand imm => imm.Value,
            RegisterOperand reg => registers[reg.Register],
            _ => throw new NotImplementedException(),
          };
          break;
        case Inc inc: registers[inc.Register] += 1; break;
        case Dec dec: registers[dec.Register] -= 1; break;
        case Jnz jnz:
          if (registers[jnz.Register] != 0) next_ip = ip + jnz.Offset;
          break;
        default:
          throw new InvalidDataException();
      };
      ip = next_ip;
    }
    return registers[Register.A].ToString();
  }

  public string SolveFirst() => Solve(
    new Dictionary<Register, int> {
      [Register.A] = 0,
      [Register.B] = 0,
      [Register.C] = 0,
      [Register.D] = 0,
    });

  public string SolveSecond() => Solve(
    new Dictionary<Register, int> {
      [Register.A] = 0,
      [Register.B] = 0,
      [Register.C] = 1,
      [Register.D] = 0,
    });
}