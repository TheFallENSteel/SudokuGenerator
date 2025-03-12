using SudokuGenerator.Args;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SudokuGenerator.Commands
{
    public static class PrintSudokuStringCommand
    {
        public static readonly string[] Aliases = ["print", "pr", "data", "d"];
        private const string Name = "print";
        private const string ShortDescription = "Prints sudoku.";
        private const string LongDescription = "Displays the sudoku in the console.";
        public static CommandInfo CommandInfo => new CommandInfo(Name, ShortDescription, LongDescription, PrintSudokuStringArgs.CommandArgsInfo);
        [ModuleInitializer]
        public static void Init()
        {
            Program.ProgramState.CommandContainer.AddCommand(new Command(Execute, CommandInfo, Aliases));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            PrintSudokuStringArgs args = new PrintSudokuStringArgs();
            args.Parse(rawArgs);
            string returnValue = "";
            if (Program.ProgramState.SudokuBuffer.Count == 0)
            {
                success = false;
                return "Sudoku buffer is empty!";
            }
            for (int i = 0; i < Program.ProgramState.SudokuBuffer.Count; i++)
            {
                if (args.Raw) returnValue += $"Sudoku {i + 1}: \n{Program.ProgramState.SudokuBuffer[i].ToString()} \n";
                else returnValue += $"Sudoku {i + 1}: \n{Program.ProgramState.SudokuBuffer[i].ToFormattedString()} \n";
            }
            success = true;
            return "Sudoku Buffer: \n" + returnValue;
        }
    }
}
