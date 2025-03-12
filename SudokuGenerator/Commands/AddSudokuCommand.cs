using SudokuGenerator.Args;
using SudokuSolver;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SudokuGenerator.Commands
{
    public static class AddSudokuCommand
    {
        public static readonly string[] Aliases = ["add", "+", "append"];
        private const string Name = "add";
        private const string ShortDescription = "Adds sudoku.";
        private const string LongDescription = "Used to add sudoku to the sudoku buffer.";
        public static CommandInfo CommandInfo => new CommandInfo(Name, ShortDescription, LongDescription, AddSudokuArgs.CommandArgsInfo);

        [ModuleInitializer]
        public static void Init()
        {
            Program.ProgramState.CommandContainer.AddCommand(new Command(Execute, CommandInfo, Aliases));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            AddSudokuArgs args = new AddSudokuArgs();
            success = false;
            args.Parse(rawArgs);
            if (args.SudokuString == null) return null;
            try
            {
                int[] sudokuData = SudokuLoader.LoadFromString(args.SudokuString, null);
                Program.ProgramState.SudokuBuffer.Add(new Sudoku(sudokuData));
                success = true;
                return $"Sudoku: {args.SudokuString}, was added successfully";
            }
            catch
            {
                return $"String was not in valid format. \nThe correct format is: \n000060030720000... ";
            }
        }
    }
}
