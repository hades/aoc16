using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc16;

[ForDay(10)]
public partial class Day10 : Solver
{
  private ImmutableDictionary<int, BotNode> bots = ImmutableDictionary<int, BotNode>.Empty;

  private ImmutableList<(int, int)> values = [];

  public void Presolve(string input)
  {
    var lines = input.Trim().Split('\n');
    var valuesBuilder = ImmutableList.CreateBuilder<(int, int)>();
    var botsBuilder = ImmutableDictionary.CreateBuilder<int, BotNode>();
    foreach (var line in lines)
    {
      if (ValueRegex().Match(line) is { Success: true } valueMatch)
      {
        valuesBuilder.Add((int.Parse(valueMatch.Groups[1].Value),
          int.Parse(valueMatch.Groups[2].Value)));
        continue;
      }

      if (BotRegex().Match(line) is { Success: true } botMatch)
      {
        var botId = int.Parse(botMatch.Groups[1].Value);
        var lowType = botMatch.Groups[2].Value == "bot" ? OutputType.Bot : OutputType.Output;
        var lowId = int.Parse(botMatch.Groups[3].Value);
        var highType = botMatch.Groups[4].Value == "bot" ? OutputType.Bot : OutputType.Output;
        var highId = int.Parse(botMatch.Groups[5].Value);
        botsBuilder[botId] =
          new BotNode(new BotOutput(lowType, lowId), new BotOutput(highType, highId));
        continue;
      }

      throw new InvalidDataException($"Invalid line: {line}");
    }

    values = valuesBuilder.ToImmutable();
    bots = botsBuilder.ToImmutable();
  }

  public string SolveFirst()
  {
    var botInputs = values;
    while (!botInputs.IsEmpty)
    {
      var nextInputs = ImmutableList.CreateBuilder<(int, int)>();
      foreach (var input in botInputs.GroupBy(x => x.Item2))
      {
        if (input.Count() == 1)
        {
          nextInputs.Add(input.First());
          continue;
        }

        var list = input.ToList();
        var inputs = (list[0].Item1, list[1].Item1);
        if (inputs.Item1 > inputs.Item2) inputs = (inputs.Item2, inputs.Item1);
        if (inputs == (17, 61)) return input.Key.ToString();
        if (bots[input.Key].Low.Type == OutputType.Bot)
          nextInputs.Add((inputs.Item1, bots[input.Key].Low.Id));
        if (bots[input.Key].High.Type == OutputType.Bot)
          nextInputs.Add((inputs.Item2, bots[input.Key].High.Id));
      }

      botInputs = nextInputs.ToImmutable();
    }

    throw new InvalidDataException("No bot compared 17 and 61");
  }

  public string SolveSecond()
  {
    var botInputs = values;
    Dictionary<int, int> outputs = new();
    while (!botInputs.IsEmpty)
    {
      var nextInputs = ImmutableList.CreateBuilder<(int, int)>();
      foreach (var input in botInputs.GroupBy(x => x.Item2))
      {
        if (input.Count() == 1)
        {
          nextInputs.Add(input.First());
          continue;
        }

        var list = input.ToList();
        var inputs = (list[0].Item1, list[1].Item1);
        if (inputs.Item1 > inputs.Item2) inputs = (inputs.Item2, inputs.Item1);
        if (bots[input.Key].Low.Type == OutputType.Bot)
          nextInputs.Add((inputs.Item1, bots[input.Key].Low.Id));
        else
          outputs[bots[input.Key].Low.Id] = inputs.Item1;
        if (bots[input.Key].High.Type == OutputType.Bot)
          nextInputs.Add((inputs.Item2, bots[input.Key].High.Id));
        else
          outputs[bots[input.Key].High.Id] = inputs.Item2;
      }

      botInputs = nextInputs.ToImmutable();
    }

    return (outputs[0] * outputs[1] * outputs[2]).ToString();
  }

  [GeneratedRegex(@"value (\d+) goes to bot (\d+)")]
  private static partial Regex ValueRegex();

  [GeneratedRegex(@"bot (\d+) gives low to (bot|output) (\d+) and high to (bot|output) (\d+)")]
  private static partial Regex BotRegex();

  private enum OutputType
  {
    Bot,
    Output
  }

  private record ValueNode(int Value, int Bot);

  private record BotOutput(OutputType Type, int Id);

  private record BotNode(BotOutput Low, BotOutput High);
}