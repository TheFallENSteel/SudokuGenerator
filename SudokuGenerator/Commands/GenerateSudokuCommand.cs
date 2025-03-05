using SudokuGenerator.Args;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public static class GenerateSudokuCommand
    {
        private static readonly string[] Aliases = ["generate", "gen", "g"];
        static GenerateSudokuCommand()
        {
            Program.ProgramState.Commands.AddCommand(new Command(Execute, aliases: Aliases));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            GenerateArgs args = new GenerateArgs();
            args.Parse(rawArgs);
            Stack<Sudoku> returnValue = new Stack<Sudoku>(args.GenerateCount);
            GenerateSolutions(args, returnValue);
            success = true;
            return $"Generated: {returnValue.Count} Sudokus";
        }

        private static void GenerateSolutions(GenerateArgs args, Stack<Sudoku> returnValue)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < args.GenerateCount - Program.ProgramState.SudokuSolutionBuffer.Count; i++)
            {
                tasks.Add(Task.Run(() => GenerateRandomSolved(args.Difficulty, returnValue)));
            }
            Task.WaitAll(tasks.ToArray());
        }
        private static void GenerateRandomSolved(int difficulty, Stack<Sudoku> returnValue) 
        {
            Sudoku? solution = null;
            lock (Program.ProgramState.SudokuSolutionBuffer)
            {
                if (Program.ProgramState.SudokuSolutionBuffer.Count > 0) 
                {
                    solution = Program.ProgramState.SudokuSolutionBuffer.Pop();
                }
            }
            solution ??= Sudoku.GenerateRandomSolved();
            solution.UnSolve(difficulty);
            lock (returnValue)
            { 
                returnValue.Push(solution);
            }
        }
    }
}
