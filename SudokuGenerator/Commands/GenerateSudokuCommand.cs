using SudokuSolver;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public static class GenerateSudokuCommand
    {
        public static readonly string[] Aliases = ["generate", "gen", "g"];
        private const string Name = "generate";
        private const string ShortDescription = "Generates sudoku.";
        private const string LongDescription = "Generates sudoku based on parameters.";
        public static CommandInfo CommandInfo => new CommandInfo(Name, ShortDescription, LongDescription, GenerateArgs.CommandArgsInfo);
        [ModuleInitializer]
        public static void Init()
        {
            Program.ProgramState.CommandContainer.AddCommand(new Command(Execute, CommandInfo, Aliases));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            GenerateArgs args = new GenerateArgs();
            args.Parse(rawArgs);
            Stack<Sudoku> returnValue = new Stack<Sudoku>(args.GenerateCount);
            GenerateSolutions(args, returnValue);
            success = true;
            Program.ProgramState.SudokuBuffer.AddRange(returnValue);
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
