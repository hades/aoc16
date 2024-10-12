using System.Collections.Immutable;

namespace aoc16;

[ForDay(6)]
public class Day06 : Solver
{
  private ImmutableList<string> input = [];

  public void Presolve(string input)
  {
    this.input = [.. input.Trim().Split('\n')];
  }

  public string SolveFirst()
  {
    return string.Concat(
      input.SelectMany(word => word.Select((c, i) => (Character: c, Position: i)))
        .GroupBy(characterEntry => characterEntry.Position)
        .Select(positionWithEntries => (Position: positionWithEntries.Key,
          BestCharacter: positionWithEntries
            .GroupBy(entry => entry.Character)
            .OrderByDescending(entryWithCharacter => entryWithCharacter.Count())
            .First()
            .Key))
        .OrderBy(position => position.Position)
        .Select(position => position.BestCharacter)
    );
  }

  public string SolveSecond()
  {
    return string.Concat(
      input.SelectMany(word => word.Select((c, i) => (Character: c, Position: i)))
        .GroupBy(characterEntry => characterEntry.Position)
        .Select(positionWithEntries => (Position: positionWithEntries.Key,
          BestCharacter: positionWithEntries
            .GroupBy(entry => entry.Character)
            .OrderBy(entryWithCharacter => entryWithCharacter.Count())
            .First()
            .Key))
        .OrderBy(position => position.Position)
        .Select(position => position.BestCharacter)
    );
  }
}