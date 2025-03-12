using SudokuGenerator.Args;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SudokuGenerator.Commands
{
    public static class SolveCommand
    {
        public static readonly string[] Aliases = ["solve"];
        private const string Name = "solve";
        private const string ShortDescription = "Solves the sudoku.";
        private const string LongDescription = "Used to solve all sudokus in the sudoku buffer.";
        public static CommandInfo CommandInfo => new CommandInfo(Name, ShortDescription, LongDescription, SolveSudokuArgs.CommandArgsInfo);

        [ModuleInitializer]
        public static void Init()
        {
            Program.ProgramState.CommandContainer.AddCommand(new Command(Execute, CommandInfo, Aliases));
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
