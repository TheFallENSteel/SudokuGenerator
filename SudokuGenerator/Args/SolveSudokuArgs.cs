using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Args
{
    public class SolveSudokuArgs : CommandArgs
    {
        public int Difficulty { get; private set; } = int.MaxValue;
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
            if (rawArgs.Count >= 1)
            {
                if (!int.TryParse(rawArgs[0], out int difficulty)) throw new ArgumentException("Invalid Difficulty argument", nameof(Difficulty));
                this.Difficulty = difficulty;
            }
        }
    }
}
