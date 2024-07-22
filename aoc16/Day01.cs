using System.Collections.Immutable;

namespace aoc16
{
    [ForDay(1)]
    public class Day01 : Solver
    {
        private ImmutableList<Tuple<char, int>> directions = ImmutableList<Tuple<char, int>>.Empty;
        private readonly ImmutableList<Tuple<int, int>> DIRECTION_MAP = ImmutableList.Create<Tuple<int, int>>(
            new(0, 1), new(1, 0), new(0, -1), new(-1, 0)
            );

        public void Presolve(string input)
        {
            directions = input.Split(", ").Select(t => ParseDirection(t)).ToImmutableList();
        }

        private static Tuple<char, int> ParseDirection(string t) => new(t[0], int.Parse(t.Substring(1)));

        public string SolveFirst()
        {
            int i = 0, j = 0;
            int dir = 0;
            foreach (var (rotation, steps) in directions)
            {
                if (rotation == 'L') dir += 3;
                if (rotation == 'R') dir += 1;
                dir %= 4;
                var (di, dj) = DIRECTION_MAP[dir];
                i += di * steps;
                j += dj * steps;
            }
            return (Math.Abs(i) + Math.Abs(j)).ToString();
        }

        public string SolveSecond()
        {
            var visited = new HashSet<Tuple<int, int>>();
            int i = 0, j = 0;
            int dir = 0;
            foreach (var (rotation, steps) in directions)
            {
                if (rotation == 'L') dir += 3;
                if (rotation == 'R') dir += 1;
                dir %= 4;
                var (di, dj) = DIRECTION_MAP[dir];
                for (int step = 0; step < steps; step++)
                {
                    i += di;
                    j += dj;
                    var key = Tuple.Create(i, j);
                    if (visited.Contains(key))
                    {
                        return (Math.Abs(i) + Math.Abs(j)).ToString();
                    }
                    visited.Add(key);
                }
            }
            throw new InvalidOperationException("no locations were visited twice");
        }
    }
}
