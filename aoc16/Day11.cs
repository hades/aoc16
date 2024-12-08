using System.Collections;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc16;

[ForDay(11)]
public partial class Day11 : Solver {
  [GeneratedRegex(@"The (?:\w+) floor contains ([^.]+)[.]")]
  private static partial Regex FloorDefinitionRe();

  [GeneratedRegex(@", (?:and )?")]
  private static partial Regex DelimiterRe();

  [GeneratedRegex(@"a (\w+)(-compatible microchip| generator)")]
  private static partial Regex ItemRe();

  private enum ItemType {
    Microchip,
    Generator,
  };

  private struct Item : IComparable<Item>, IEquatable<Item> {
    public ItemType Type;
    public string Element;

    public override string ToString() {
      char t = Type == ItemType.Microchip ? 'm' : 'g';
      return $"{Element[0]}{t}";
    }

    public int CompareTo(Item other) {
      int c = Type.CompareTo(other.Type);
      if (c != 0) return c;
      return Element.CompareTo(other.Element);
    }

    public bool Equals(Item other) {
      return Type == other.Type && Element == other.Element;
    }
  }

  private struct Floor: IEquatable<Floor> {
    public ImmutableSortedSet<Item> Items;

    public bool Equals(Floor other) {
      return Items.SequenceEqual(other.Items);
    }

    public override string ToString() {
      return $"[{String.Join(" ", Items.Select(item => item.ToString()))}]";
    }
  }

  private ImmutableArray<Floor> floors;

  public void Presolve(string input) {
    floors = input.Trim().Split('\n').Select(line => {
      if (FloorDefinitionRe().Match(line) is not { Success: true } floorDefMatch) {
        throw new InvalidDataException($"cannot parse line: {line}");
      }
      var items = DelimiterRe().Split(floorDefMatch.Groups[1].Value);
      List<Item> parsed_items = [];
      foreach (var item in items) {
        if (item == "nothing relevant") break;
        if (ItemRe().Match(item) is not { Success: true } itemDefMatch) {
          throw new InvalidDataException($"cannot parse item: {item}");
        }
        var element = itemDefMatch.Groups[1].Value;
        if (itemDefMatch.Groups[2].Value == " generator") {
          parsed_items.Add(new Item { Element = element, Type = ItemType.Generator });
        } else {
          parsed_items.Add(new Item { Element = element, Type = ItemType.Microchip});
        }
      }
      return new Floor {
        Items = [..parsed_items],
      };
    }).ToImmutableArray();
  }

  private static bool IsEndState(long state) {
    return (state & 0x00FFFFFFFFFFC000L) == 0;
  }

  private static IEnumerable<long> AdjacentStates(long state) {
    int floor = (int)(state >> 56);
    long state_without_floor = (state & 0x00FFFFFFFFFFFFFFL);
    for (long i = 0, item_mask = 1L << ((3 - floor) * 14); i < 14; i++, item_mask <<= 1) {
      if ((state & item_mask) == 0) continue;
      if (floor > 0) {
        long after_moving_one = (state_without_floor | (item_mask << 14) | ((long)(floor - 1) << 56)) & (~item_mask);
        yield return after_moving_one;
        for (long j = i + 1, second_item_mask = item_mask << 1; j < 14; j++, second_item_mask <<= 1) {
          if ((state & second_item_mask) == 0) continue;
          yield return (after_moving_one | (second_item_mask << 14)) & (~second_item_mask);
        }
      }
      if (floor < 3) {
        long after_moving_one = (state_without_floor | (item_mask >> 14) | ((long)(floor + 1) << 56)) & (~item_mask);
        yield return after_moving_one;
        for (long j = i + 1, second_item_mask = item_mask << 1; j < 14; j++, second_item_mask <<= 1) {
          if ((state & second_item_mask) == 0) continue;
          yield return (after_moving_one | (second_item_mask >> 14)) & (~second_item_mask);
        }
      }
    }
  }

  private static bool IsValidState(long state) {
    long microchip_mask = 0x7F;
    long generator_mask = 0x3F80;
    for (int i = 0; i < 4; ++i) {
      long microchips = state & microchip_mask;
      long generators = (state & generator_mask) >> 7;
      if (generators != 0 && (microchips & (~generators)) != 0) return false;
      microchip_mask <<= 14;
      generator_mask <<= 14;
    }
    return true;
  }

  private string Solve(bool extra_items) {
    long initial_state = 0;
    List<string> elements = [];
    bool first_floor = true;
    foreach (var floor in floors) {
      initial_state <<= 14;
      var items = floor.Items.ToList();
      if (first_floor && extra_items) {
        items.Add(new Item { Element = "elerium", Type = ItemType.Generator });
        items.Add(new Item { Element = "elerium", Type = ItemType.Microchip });
        items.Add(new Item { Element = "dilithium", Type = ItemType.Generator });
        items.Add(new Item { Element = "dilithium", Type = ItemType.Microchip });
        first_floor = false;
      }
      foreach (var item in items) {
        int item_element = elements.IndexOf(item.Element);
        if (item_element == -1) {
          item_element = elements.Count;
          elements.Add(item.Element);
        }
        long item_mask = 1L << item_element;
        if (item.Type == ItemType.Generator) item_mask <<= 7;
        initial_state |= item_mask;
      }
    }
    HashSet<long> states = [initial_state];
    HashSet<long> visited_states = [];
    int steps = 0;
    while (true) {
      if (states.Count == 0) throw new InvalidDataException($"no solution found after {steps} steps");
      if (states.Where(IsEndState).Any()) break;
      steps++;
      foreach (var state in states) visited_states.Add(state);
      var new_states = states.SelectMany(AdjacentStates).Where(IsValidState).Except(visited_states).ToHashSet();
      states = new_states;
    }
    return steps.ToString();
  }

  public string SolveFirst() => Solve(false);
  public string SolveSecond() => Solve(true);
}