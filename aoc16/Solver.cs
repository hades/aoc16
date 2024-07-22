using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc16
{
    /**
     * Interface for solvers for problems for individual AoC daily puzzles.
     */
    public interface Solver
    {
        void Presolve(string input);
        string SolveFirst();
        string SolveSecond();
    }
}
