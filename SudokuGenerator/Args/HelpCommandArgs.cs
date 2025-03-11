using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.CommandString = CommandArgs.ParseArg(rawArgs, 0, "quickStart");
        }
    }
}
