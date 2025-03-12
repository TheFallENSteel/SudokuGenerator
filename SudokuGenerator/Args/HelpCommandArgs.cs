using SudokuGenerator.Commands;
using System.Collections.Generic;

namespace SudokuGenerator.Args
{
    public class HelpCommandArgs : CommandArgs
    {
        public string? CommandString { get; private set; }
        public static CommandArgsInfo CommandArgsInfo { get; } = new CommandArgsInfo(
        [
            new ParameterInfo(
                "Command", "string", "Command you need to help with " +
                "\nDefault is quickGuide")
        ]
    );
        public override void Parse(List<string> rawArgs)
        {
            rawArgs = CommandArgs.ProcessArgs(rawArgs);
            this.CommandString = CommandArgs.ParseArg(rawArgs, 0, QuickStart.Aliases[0]);
        }
    }
}
