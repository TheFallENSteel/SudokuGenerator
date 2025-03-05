using SudokuGenerator.Args;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public static class ExitCommand
    {
        private static readonly string[] Aliases = ["exit", "ex", "e", "stop", "terminate", "end", "quit", "leave", "q"];
        static ExitCommand()
        {
            Program.ProgramState.Commands.AddCommand(new Command(Execute, aliases: Aliases));
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
