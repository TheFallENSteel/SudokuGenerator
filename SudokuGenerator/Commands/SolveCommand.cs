using SudokuGenerator.Args;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public static class SolveCommand
    {
        static SolveCommand()
        {
            Program.ProgramState.Commands.AddCommand(new Command(Execute, ["solve"]));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            string outputString = "Solved Sudokus: \n";
            SolveSudokuArgs args = new SolveSudokuArgs();
            args.Parse(rawArgs);
            success = false;
            try
            {
                foreach (Sudoku sudoku in Program.ProgramState.SudokuBuffer)
                {
                    SolveAndPrintResults(args, sudoku, out string formattedSolvedSudoku);
                    outputString += formattedSolvedSudoku + "\n";
                }
            }
            catch (Exception e)
            {
                return $"Sudoku Error: {e.Message}";
            }
            success = true;
            return outputString + $"Sudokus were solved successfully.";
        }
        private static Sudoku? SolveAndPrintResults(SolveSudokuArgs args, Sudoku sudoku, out string formattedSolvedSudoku)
        {
            formattedSolvedSudoku = "";
            Sudoku? result = sudoku.Solve(args.Difficulty);
            if (result != null) formattedSolvedSudoku = result.ToFormattedString();
            return result;
        }
    }
}
