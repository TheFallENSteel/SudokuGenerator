using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuGenerator.Commands
{
    public class Command
    {
        public delegate string? ExecuteCommand(List<string> args, out bool success);
        private ExecuteCommand ExecuteMethod { get; }
        public string[] Aliases { get; }
        public CommandInfo CommandInfo { get; }

        public Command(ExecuteCommand command, CommandInfo commandArgsInfo, string[] aliases)
        {
            ExecuteMethod = command;
            Aliases = aliases.Select(a => a.ToLower()).ToArray();
            CommandInfo = commandArgsInfo;
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
