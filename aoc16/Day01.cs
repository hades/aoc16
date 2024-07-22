using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;

namespace aoc16
{
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
            throw new NotImplementedException();
        }
    }
}
