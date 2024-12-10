namespace aoc16;

[ForDay(16)]
public partial class Day16 : Solver {
  private string data;

  public void Presolve(string input) {
    data = input.Trim();
  }

  private string DragonExtend(string input) => input + "0" + new String(input.Reverse().Select(ch => (char)('0' + (1 - (ch - '0')))).ToArray());

  public string SolveFirst() {
    string generated_data = data;
    while (generated_data.Length < 272) {
      generated_data = DragonExtend(generated_data);
    }
    generated_data = generated_data.Substring(0, 272);
    string checksum = generated_data;
    while (checksum.Length % 2 == 0) {
      checksum = new String(Enumerable.Range(0, checksum.Length / 2).Select(idx => checksum[idx * 2] == checksum[idx * 2 + 1] ? '1' : '0').ToArray());
    }
    return checksum;
  }

  public string SolveSecond() {
    string generated_data = data;
    while (generated_data.Length < 35651584) {
      generated_data = DragonExtend(generated_data);
    }
    generated_data = generated_data.Substring(0, 35651584);
    string checksum = generated_data;
    while (checksum.Length % 2 == 0) {
      checksum = new String(Enumerable.Range(0, checksum.Length / 2).Select(idx => checksum[idx * 2] == checksum[idx * 2 + 1] ? '1' : '0').ToArray());
    }
    return checksum;
  }
}