using SudokuGenerator.Args;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public static class ClearCommand
    {
        private static readonly string[] Aliases = ["clear", "clr", "cl", "delete", "del", "flush"];
        static ClearCommand()
        {
            Program.ProgramState.Commands.AddCommand(new Command(Execute, EmptyArgs.CommandArgsInfo, Aliases, ""));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            int count = Program.ProgramState.SudokuBuffer.Count;
            Program.ProgramState.SudokuBuffer.Clear();
            success = true;
            return $"Clear deleted {count} elements";
        }
    }
}
