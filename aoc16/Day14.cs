using System.Security.Cryptography;

namespace aoc16;

[ForDay(14)]
public partial class Day14 : Solver {
  private string salt;

  public void Presolve(string input) {
    salt = input.Trim();
  }

  private IEnumerable<(int, string)> GenerateSuitableKeys(int cycles, int n) {
    Dictionary<char, List<int>> last_triplet_indices = [];
    Deque<string> last_keys = [];
    int max_index = -1;
    List<(int, string)> keys = [];
    SortedSet<int> generated_keys = [];
    foreach (var (index, key) in GenerateKeys(cycles)) {
      if (max_index >= 0 && index - max_index > 1001) break;
      last_keys.AddFront(key);
      foreach (var (ch, reps) in CountRepetitions(key)) {
        if (reps < 5) continue;
        if (!last_triplet_indices.ContainsKey(ch)) continue;
        for (int i = last_triplet_indices[ch].Count - 1; i >= 0; i--) {
          int last_triplet_index = last_triplet_indices[ch][i];
          if (generated_keys.Contains(last_triplet_index)) continue;
          int offset = index - last_triplet_index;
          if (offset > 1000) break;
          keys.Add((last_triplet_index, last_keys[offset]));
          generated_keys.Add(last_triplet_index);
          if (keys.Count >= n) {
            max_index = keys.Select(k => k.Item1).Max();
          }
        }
      }
      if (CountRepetitions(key).FirstOrDefault() is var (triplet_ch, triplet_reps) && triplet_reps >= 3) {
        last_triplet_indices.TryAdd(triplet_ch, []);
        last_triplet_indices[triplet_ch].Add(index);
      }
      if (last_keys.Count > 1000) last_keys.RemoveBack();
    }
    keys.Sort();
    return keys.Take(64);
  }

  private IEnumerable<(char, int)> CountRepetitions(string key) {
    int start_index = -1;
    char ch = '\0';
    for (int i = 0; i <= key.Length; i++) {
      if (start_index >= 0) {
        if (i >= key.Length || key[i] != ch) {
          int reps = i - start_index;
          if (reps >= 3) yield return (ch, reps);
        } else {
          continue;
        }
      }
      start_index = i;
      if (i < key.Length) ch = key[i];
    }
  }

  private IEnumerable<(int, string)> GenerateKeys(int cycles) {
    for (int index = 0; ; index++) {
      string key = Convert.ToHexString(MD5.HashData(
        System.Text.Encoding.ASCII.GetBytes(salt + index.ToString()))).ToLower();
      for (int i = 0; i < cycles; i++) {
        key = Convert.ToHexString(MD5.HashData(System.Text.Encoding.ASCII.GetBytes(key))).ToLower();
      }
      yield return (index, key);
    }
  }

  public string SolveFirst() => GenerateSuitableKeys(0, 64).Last().Item1.ToString();
  public string SolveSecond() => GenerateSuitableKeys(2016, 64).Last().Item1.ToString();
}