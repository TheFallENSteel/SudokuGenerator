using SudokuGenerator.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SudokuGenerator.Commands
{
    public static class HelpPositionalCommand
    {
        private const string Name = "help";
        private const string ShortDescription = "Used to get info about command and its parameters.";
        private const string LongDescription = "Can be used as normal command (help \"commandName\") " +
            $"\nor positional command (\"commandName\" ?help) " +
            "\nstandard command returns info about command and positional command returns parameter info.";
        public static readonly string[] Aliases = ["help", "h"];
        public static CommandInfo CommandInfo => new CommandInfo(Name, ShortDescription, LongDescription, HelpCommandArgs.CommandArgsInfo);
        public static CommandsContainer CommandContainer => Program.ProgramState.CommandContainer;
        [ModuleInitializer]
        public static void Init()
        {
            Program.ProgramState.CommandContainer.AddCommand(new Command(Execute, CommandInfo, Aliases));
            Program.ProgramState.CommandContainer.AddCommand(new PositionalCommand(ExecutePositional, CommandInfo, Aliases));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            success = false;
            HelpCommandArgs args = new HelpCommandArgs();
            args.Parse(rawArgs);
            if (!CommandContainer.GetCommand(args.CommandString, out Command targetCommand))
            {
                return $"No command with alias \"{args.CommandString}\" exists!";
            }
            success = true;
            return "From help command:\n " + targetCommand.CommandInfo.FullDescription;
        }
        public static string? ExecutePositional(List<string> rawArgs, out bool success)
        {
            //-1 for index, -1 for not counting command itself
            int index = rawArgs.TakeWhile(s => !Regex.IsMatch(s, @"-help\b")).Count() - 2;
            success = false;
            HelpCommandArgs args = new HelpCommandArgs();
            args.Parse(rawArgs);
            if (!CommandContainer.GetCommand(args.CommandString, out Command targetCommand))
            {
                return $"No command with alias \"{args.CommandString}\" exists!";
            }
            success = true;
            return "From help command:\n" + targetCommand.CommandInfo.ShortDescription + "\n" + targetCommand.CommandInfo.CommandArgsInfo.GetHelpString(index);
        }
    }
}
