using System.Collections.Generic;

namespace SudokuGenerator.Commands
{
    public class PositionalCommand
    {
        public const char PositionalCommandPrefix = '?';
        public delegate string? ExecutePositionalCommandCommand(List<string> args, out bool success);
        public string[] Aliases { get; }
        public CommandInfo CommandInfo { get; }
        private ExecutePositionalCommandCommand ExecutePositionalMethod { get; }
        public PositionalCommand(ExecutePositionalCommandCommand positionalCommand, CommandInfo commandInfo, string[] aliases)
        {
            this.ExecutePositionalMethod = positionalCommand;
            this.CommandInfo = commandInfo;
            this.Aliases = aliases;
        }
        public virtual string? ExecutePositional(List<string> args, out bool success)
        {
            return ExecutePositionalMethod.Invoke(args, out success);
        }
    }
}
