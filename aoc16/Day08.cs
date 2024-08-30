using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc16
{
    [ForDay(8)]
    public partial class Day08 : Solver
    {
        abstract record Command;
        record Rect(int Width, int Height) : Command;
        record RotateRow(int Row, int Amount) : Command;
        record RotateColumn(int Column, int Amount) : Command;

        ImmutableList<Command> input = [];

        [GeneratedRegex(@"rect (\d+)x(\d+)")]
        private static partial Regex RectRegex();

        [GeneratedRegex(@"rotate row y=(\d+) by (\d+)")]
        private static partial Regex RotateRowRegex();

        [GeneratedRegex(@"rotate column x=(\d+) by (\d+)")]
        private static partial Regex RotateColumnRegex();

        const int Width = 50;
        const int Height = 6;
        const int LetterWidth = 5;

        private readonly Dictionary<string, string> Letters = new()
        {
            ["####.#....###..#....#....#...."] = "F",
            [".##..#..#.#....#.##.#..#..###."] = "G",
            ["#..#.#..#.####.#..#.#..#.#..#."] = "H",
            [".##..#..#.#..#.#..#.#..#..##.."] = "O",
            ["###..#..#.#..#.###..#....#...."] = "P",
            [".###.#....#.....##.....#.###.."] = "S",
            ["####....#...#...#...#....####."] = "Z",
            // I don't know what the remaining letters look like, so encode your own here if needed.
        };

        static Command ParseCommand(string input) {
            if (RectRegex().Match(input) is { Success: true } matchRect) {
                return new Rect(int.Parse(matchRect.Groups[1].Value), int.Parse(matchRect.Groups[2].Value));
            }
            if (RotateRowRegex().Match(input) is { Success: true } matchRow) {
                return new RotateRow(int.Parse(matchRow.Groups[1].Value), int.Parse(matchRow.Groups[2].Value));
            }
            if (RotateColumnRegex().Match(input) is { Success: true } matchColumn) {
                return new RotateColumn(int.Parse(matchColumn.Groups[1].Value), int.Parse(matchColumn.Groups[2].Value));
            }
            throw new InvalidDataException($"Invalid input: {input}");
        }

        bool[,] lights = new bool[Height, Width];

        public void Presolve(string input)
        {
            var commands = input.Trim().Split('\n').Select(ParseCommand).ToImmutableList();
            lights = new bool[Height, Width];
            foreach (var command in commands) {
                if (command is Rect rect) {
                    for (int row = 0; row < rect.Height; ++row) {
                        for (int column = 0; column < rect.Width; ++column) {
                            lights[row, column] = true;
                        }
                    }
                } else if (command is RotateRow rotateRow) {
                    bool[] row = new bool[Width];
                    for (int column = 0; column < Width; ++column) {
                        row[(column + rotateRow.Amount) % Width] = lights[rotateRow.Row, column];
                    }
                    for (int column = 0; column < Width; ++column) {
                        lights[rotateRow.Row, column] = row[column];
                    }
                } else if (command is RotateColumn rotateColumn) {
                    bool[] column = new bool[Height];
                    for (int row = 0; row < Height; ++row) {
                        column[(row + rotateColumn.Amount) % Height] = lights[row, rotateColumn.Column];
                    }
                    for (int row = 0; row < Height; ++row) {
                        lights[row, rotateColumn.Column] = column[row];
                    }
                }
            }
        }

        public string SolveFirst()
        {
            int count = 0;
            foreach (var light in lights) {
                if (light) {
                    count += 1;
                }
            }
            return count.ToString();
        }

        public string SolveSecond()
        {
            string word = "";
            for (int letter = 0; letter < Width/LetterWidth; letter++) {
                string[] letterRows = new string[Height];
                for (int row = 0; row < Height; ++row) {
                    letterRows[row] = "";
                    for (int column = LetterWidth * letter; column < LetterWidth * (letter + 1); ++column) {
                        letterRows[row] += lights[row, column] ? "#" : ".";
                    }
                }
                string letterEncoded = String.Join("", letterRows);
                if (Letters.TryGetValue(letterEncoded, out string? letterChar) && letterChar != null) {
                    word += letterChar;
                } else {
                    throw new InvalidDataException($"Unable to recognize this letter:\n\n{String.Join("\n", letterRows)}\n\n" + 
                                                   "Please encode it in the Letters dictionary. Here's a template:\n\n" +
                                                   $"            [\"{letterEncoded}\"] = \"X\",\n");
                }
            }
            return word;
        }
    }
}

