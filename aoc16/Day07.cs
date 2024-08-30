using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc16
{
    [ForDay(7)]
    public partial class Day07 : Solver
    {
        ImmutableList<(ImmutableList<string> outside, ImmutableList<string> inside)> parsed;

        [GeneratedRegex(@"^(?:(\w+)\[(\w+)\])*(\w+)$")]
        private static partial Regex AddressRegex();

        private static bool HasAbba(string s)
        {
            for (int i = 0; i < s.Length - 3; i++)
            {
                if (s[i] == s[i + 3] && s[i + 1] == s[i + 2] && s[i] != s[i + 1])
                {
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<string> Abas(string s)
        {
            for (int i = 0; i < s.Length - 2; i++)
            {
                if (s[i] == s[i + 2] && s[i] != s[i + 1])
                {
                    yield return s.Substring(i, 3);
                }
            }
        }

        public void Presolve(string input)
        {
            var lines = input.Trim().Split("\n");
            this.parsed = lines.Select(line => {
                var m = AddressRegex().Match(line);
                if (!(m?.Success ?? false)) throw new InvalidDataException($"Invalid input: {line}");
                var outside = m.Groups[1].Captures.Union(m.Groups[3].Captures).Select(c => c.Value).ToImmutableList();
                var inside = m.Groups[2].Captures.Select(c => c.Value).ToImmutableList();
                return (outside, inside);
            }).ToImmutableList();
        }

        public string SolveFirst()
        {
            return parsed.Where(pair => pair.outside.Any(HasAbba) && !pair.inside.Any(HasAbba)).Count().ToString();
        }

        public string SolveSecond()
        {
            return parsed.Select(line => {
                var abas = line.outside.SelectMany(Abas).ToImmutableList();
                var babs = line.inside.SelectMany(Abas).ToImmutableList();
                return (abas, babs);
            }).Where(item => item.abas.Any(aba => item.babs.Contains($"{aba[1]}{aba[0]}{aba[1]}"))).Count().ToString();
        }
    }
}

