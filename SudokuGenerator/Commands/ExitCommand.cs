using SudokuGenerator.Args;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SudokuGenerator.Commands
{
    public static class ExitCommand
    {
        public static readonly string[] Aliases = ["exit", "ex", "e", "stop", "terminate", "end", "quit", "leave", "q"];
        private const string Name = "exit";
        private const string ShortDescription = "Exits the program.";
        private const string LongDescription = "Used to exit the program after user confirmation.";
        public static CommandInfo CommandInfo => new CommandInfo(Name, ShortDescription, LongDescription, EmptyArgs.CommandArgsInfo);

        [ModuleInitializer]
        public static void Init()
        {
            Program.ProgramState.CommandContainer.AddCommand(new Command(Execute, CommandInfo, Aliases));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            success = false;
            Console.WriteLine("Are you sure you want to exit the app? \n(All sudokus in buffer will be lost!) \nPress 'y' to exit");
            string? answer = Console.ReadLine();
            if (answer != null && answer.Trim().ToLower() != "y") return $"Continueing the program...";
            success = true;
            return $"See you next timeExiting the program...";
        }
    }
}
