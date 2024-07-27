using System.Collections.Immutable;

namespace aoc16
{
    [ForDay(3)]
    public class Day03 : Solver
    {
        private ImmutableList<Tuple<int, int, int>> triangles = [];

        public void Presolve(string input)
        {
            triangles = input
                .Trim()
                .Split("\n").Select(line => {
                    var ints = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    return new Tuple<int, int, int>(ints[0], ints[1], ints[2]);
                })
                .ToImmutableList();
        }

        private bool IsTriangle(int a, int b, int c) {
            if (a + b <= c) return false;
            if (a + c <= b) return false;
            if (b + c <= a) return false;
            return true;
        }

        public string SolveFirst()
        {
            return triangles.Select(triangle => {
                var (a, b, c) = triangle;
                return IsTriangle(a, b, c) ? 1 : 0;
            }).Sum().ToString();
        }

        public string SolveSecond()
        {
            int count = 0;
            for (int i = 0; i < triangles.Count; i += 3) {
                if (IsTriangle(triangles[i].Item1, triangles[i + 1].Item1, triangles[i + 2].Item1)) count++;
                if (IsTriangle(triangles[i].Item2, triangles[i + 1].Item2, triangles[i + 2].Item2)) count++;
                if (IsTriangle(triangles[i].Item3, triangles[i + 1].Item3, triangles[i + 2].Item3)) count++;
            }
            return count.ToString();
        }
    }
}
