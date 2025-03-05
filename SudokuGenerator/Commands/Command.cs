using SudokuGenerator.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public class Command
    {
        public delegate string? ExecuteCommand(List<string> args, out bool success);
        private ExecuteCommand ExecuteMethod { get; }
        public string[] Aliases { get; }
        public string HelpString { get; }
        public ParameterInfo ParameterInfo { get; }
        public CommandArgsInfo CommandArgsInfo { get; }

        public Command(ExecuteCommand command, CommandArgsInfo commandArgsInfo, string[] aliases, string helpString)
        {
            ExecuteMethod = command;
            Aliases = aliases.Select(a => a.ToLower()).ToArray();
            HelpString = helpString;
            CommandArgsInfo = commandArgsInfo;
        }
        public bool IsAlias(string alias)
        {
            return Aliases.Contains(alias);
        }
        public virtual string? Execute(List<string> args, out bool success)
        {
            return ExecuteMethod.Invoke(args, out success);
        }
    }
}
