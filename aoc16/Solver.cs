namespace aoc16;

[AttributeUsage(AttributeTargets.Class)]
public class ForDayAttribute : Attribute
{
  public readonly int day;

  public ForDayAttribute(int day)
  {
    this.day = day;
  }
}

/**
 * Interface for solvers for problems for individual AoC daily puzzles.
 */
public interface Solver
{
  void Presolve(string input);
  string SolveFirst();
  string SolveSecond();

  static Solver GetSolverForDay(int day)
  {
    foreach (var type in typeof(Solver).Assembly.DefinedTypes)
    {
      if (Attribute.GetCustomAttribute(type, typeof(ForDayAttribute)) is not ForDayAttribute
          attribute) continue;
      if (attribute.day == day)
      {
        if (Activator.CreateInstance(type) is not Solver instance)
          throw new InvalidOperationException($"unable to instantiate solver {type} for day {day}");
        return instance;
      }
    }

    throw new InvalidDataException($"solver for day {day} not found");
  }
}