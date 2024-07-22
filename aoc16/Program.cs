using aoc16;
using CommandLine;
using CommandLine.Text;
using System.Diagnostics;
using System.Drawing;
using Console = Colorful.Console;

public class Program
{
    class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Enable verbose output.")]
        public bool Verbose { get; set; }
        [Option('s', "submit", Required = false, HelpText = "Automatically submit the answer.")]
        public bool Submit { get; set; }
        [Option('c', "cookie", Required = false, Default = "", HelpText = "Session cookie to download input data and to submit answer.")]
        public required string Cookie { get; set; }
        [Option('2', "second_only", Required = false, HelpText = "Only solve the second part of the puzzle.")]
        public bool SecondOnly { get; set; }

        [Value(0)]
        public required IEnumerable<int> Days { get; set; }

        [Usage(ApplicationAlias = "aoc16")]
        public static IEnumerable<Example> Examples { get
            {
                yield return new Example("Solve one day (14th puzzle).", new Options {
                    Days = [14], Cookie = "" });
                yield return new Example("Solve several days.", new Options {
                    Days = [14, 16, 20], Cookie = "" });
                yield return new Example("Solve and submit answer.", new Options {
                    Days = [14], Cookie = "deadbeef", Submit = true });
            }
        }
    }

    static int Main(Options opts)
    {
        if (!opts.Days.Any())
        {
            Console.WriteLine("no days requested, exiting");
        }
        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "aoc16/1 (https://github.com/hades/aoc16)");
        httpClient.DefaultRequestHeaders.Add("cookie", $"session={opts.Cookie}");
        int errors = 0;
        foreach (var day in opts.Days)
        {
            try
            {
                var solver = Solver.GetSolverForDay(day);
                var input = GetInput(day, httpClient);
                Console.WriteLine($"presolving day {day}...");
                var presolveTime = TimePresolve(solver, input);
                Console.WriteLine($"presolving took {presolveTime}");
                if (!opts.SecondOnly)
                {
                    Console.WriteLine($"solving first part of day {day}...");
                    var (result, solveTime) = TimeSolve(solver, input, false);
                    Console.WriteLine($"answer is {result} (took {solveTime})");
                }
                {
                    Console.WriteLine($"solving second part of day {day}...");
                    var (result, solveTime) = TimeSolve(solver, input, true);
                    Console.WriteLine($"answer is {result} (took {solveTime})");
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
                if (opts.Verbose) { Console.WriteLine(ex.StackTrace); }
                errors++;
            }
        }
        return errors;
    }

    private static (string, TimeSpan) TimeSolve(Solver solver, string input, bool second)
    {
        var watch = Stopwatch.StartNew();
        var result = second ? solver.SolveSecond() : solver.SolveFirst();
        return (result, watch.Elapsed);
    }

    private static string GetInput(int day, HttpClient httpClient)
    {
        return httpClient.GetStringAsync($"https://adventofcode.com/2016/day/{day}/input").GetAwaiter().GetResult();
    }

    private static TimeSpan TimePresolve(Solver solver, string input)
    {
        var watch = Stopwatch.StartNew();
        solver.Presolve(input);
        return watch.Elapsed;
    }

    static int Main(string[] args)
    {
        Console.WriteAscii("AoC 2016", ColorTranslator.FromHtml("#fad6ff"));
        return Parser.Default.ParseArguments<Options>(args).MapResult(
            opts => Main(opts),
            _ => 1);
    }

    private static void WriteError(string? message) { Console.WriteLine(message); }
}
