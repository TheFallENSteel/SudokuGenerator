using SudokuGenerator.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public static class HelpPositionalCommand
    {
        private static readonly string[] Aliases = ["help", "help"];
        static HelpPositionalCommand()
        {
            Program.ProgramState.Commands.AddCommand(new PositionalCommand(ExecutePositional, HelpCommandArgs.CommandArgsInfo, Execute, Aliases));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            int count = Program.ProgramState.SudokuBuffer.Count;
            Program.ProgramState.SudokuBuffer.Clear();
            success = true;
            return $"Clear deleted {count} elements";
        }
        public static string? ExecutePositional(List<string> rawArgs, out bool success)
        {
            int count = Program.ProgramState.SudokuBuffer.Count;
            Program.ProgramState.SudokuBuffer.Clear();
            success = true;
            return $"Clear deleted {count} elements";
        }
    }
}
