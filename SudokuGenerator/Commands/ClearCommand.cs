using SudokuGenerator.Args;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SudokuGenerator.Commands
{
    public static class ClearCommand
    {
        private const string Name = "clear";
        private const string ShortDescription = "Clears sudokus.";
        private const string LongDescription = "Clears all sudokus from the buffer permanently after user confirmation.";
        public static CommandInfo CommandInfo => new CommandInfo(Name, ShortDescription, LongDescription, EmptyArgs.CommandArgsInfo);

        public static readonly string[] Aliases = ["clear", "clr", "cl", "delete", "del", "flush"];

        [ModuleInitializer]
        public static void Init()
        {
            Program.ProgramState.CommandContainer.AddCommand(new Command(Execute, CommandInfo, Aliases));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            int count = Program.ProgramState.SudokuBuffer.Count;
            Console.WriteLine("Do you really wish to delete all saved sudokus? \n[y/n]");
            if (Console.ReadLine() == "y")
            {
                success = true;
                Program.ProgramState.SudokuBuffer.Clear();
                return $"Clear deleted {count} elements";
            }
            else
            {
                success = false;
                return "Clear command stopped by the user";
            }

        }
    }
}
