namespace aoc16;

[ForDay(18)]
public partial class Day18 : Solver {
  private string data;

  public void Presolve(string input) {
    data = input.Trim();
  }

  private string Solve(int total_steps) {
    long counter = 0;
    string last_row = data;
    for (int i = 0; i < total_steps - 1; ++i) {
      counter += last_row.Where(ch => ch == '.').Count();
      List<char> next_row = [];
      for (int j = 0; j < last_row.Length; ++j) {
        bool left_trap = j > 0 && last_row[j-1] == '^';
        bool centre_trap = last_row[j] == '^';
        bool right_trap = j < (last_row.Length - 1) && last_row[j + 1] == '^';
        next_row.Add(
          left_trap && centre_trap && !right_trap ||
          centre_trap && right_trap && !left_trap ||
          left_trap && !centre_trap && !right_trap ||
          right_trap && !centre_trap && !left_trap ?
          '^' : '.'
          );
      }
      last_row = new string([.. next_row]);
    }
    counter += last_row.Where(ch => ch == '.').Count();
    return counter.ToString();
  }

  public string SolveFirst() => Solve(40);
  public string SolveSecond() => Solve(400000);
}