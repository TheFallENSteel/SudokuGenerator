using SudokuSolver;
using System.Collections.Generic;
using System.Linq;

namespace SudokuGenerator.Args
{
    public class SolveSudokuArgs : CommandArgs
    {
        public int Difficulty { get; private set; }
        public static CommandArgsInfo CommandArgsInfo { get; } = new CommandArgsInfo([
            new ParameterInfo(
                "MaxDifficulty",
                "Integer",
                "   Determines maximum difficulty that the solver can use to solve the puzzle. " +
                $"\n<1: Disallows all solving methods" +
                $"\n>={Sudoku.MAX_DIFFICULTY}: Allows all solving methods" +
                $"\nDefault is all solving methods allowed.")
            ]);

        public override void Parse(List<string> rawArgs)
        {
            rawArgs = rawArgs.Select(arg => arg.Trim().Trim('"').Trim('\'')).ToList();
            rawArgs = CommandArgs.ProcessArgs(rawArgs);
            this.Difficulty = CommandArgs.ParseArg(rawArgs, 0, int.MaxValue);
        }
    }
}
