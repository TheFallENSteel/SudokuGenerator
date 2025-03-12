using SudokuGenerator.Args;
using SudokuSolver;
using System.Collections.Generic;

namespace SudokuGenerator
{
    public class GenerateArgs : CommandArgs
    {
        public int GenerateCount { get; private set; }
        public int Difficulty { get; private set; }
        public static CommandArgsInfo CommandArgsInfo { get; } = new CommandArgsInfo([
                new ParameterInfo(
                    "Difficulty",
                    "int",
                    "Determines the maximum difficulty of the generated sudoku. "
                    +$"\n<1: Disallows all solving methods" +
                    $"\n>={Sudoku.MAX_DIFFICULTY}: Allows all solving methods" +
                    "\nDefault value is 1."),
                new ParameterInfo(
                    "Count",
                    "positive int",
                    "Determines how many sudokus should be generated. " +
                    "\nDefault value is 1.")
            ]);
        public override void Parse(List<string> rawArgs)
        {
            rawArgs = CommandArgs.ProcessArgs(rawArgs);
            this.Difficulty = CommandArgs.ParseArg(rawArgs, 0, int.MaxValue);
            this.GenerateCount = CommandArgs.ParseArg(rawArgs, 1, 1);
        }
    }
}
