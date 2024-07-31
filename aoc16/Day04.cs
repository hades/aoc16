using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc16
{
    [ForDay(4)]
    public partial class Day04 : Solver
    {
        private ImmutableList<(string Name, int Sector, string Checksum)> rooms = [];

        [GeneratedRegex(@"^([-a-z]+)-(\d+)\[([a-z]+)\]$")]
        private static partial Regex RoomDescriptorRegex();

        public void Presolve(string input)
        {
            rooms = input
                .Trim()
                .Split("\n").Select(line => {
                    var m = RoomDescriptorRegex().Match(line);
                    if (!m.Success) throw new Exception($"Invalid input: {line}");
                    return (m.Groups[1].Value, int.Parse(m.Groups[2].Value), m.Groups[3].Value);
                })
                .ToImmutableList();
        }

        public string SolveFirst()
        {
            return rooms.Select(room => {
                var computedChecksum = room.Name
                    .Where(c => c != '-')
                    .GroupBy(c => c)
                    .Select(g => (Letter: g.Key, Incidence: g.Count()))
                    .OrderByDescending(t => t.Incidence)
                    .ThenBy(t => t.Letter)
                    .Take(5)
                    .Select(t => t.Letter)
                    .ToArray();
                return computedChecksum.SequenceEqual(room.Checksum) ? room.Sector : 0;
            }).Sum().ToString();
        }

        public string SolveSecond()
        {
            return rooms.Select(room => {
                var decryptedName = room.Name
                    .Select(c => c == '-' ? ' ' : (char)('a' + (c - 'a' + room.Sector) % 26))
                    .ToArray();
                return "northpole object storage".SequenceEqual(decryptedName) ? room.Sector : 0;
            }).Sum().ToString();
        }
    }
}
