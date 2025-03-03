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
        public ExecuteCommand ExecuteMethod { get; }
        public readonly string[] Aliases;
        public Command(ExecuteCommand command, string[] aliases)
        {
            Aliases = aliases.Select(a => a.ToLower()).ToArray();
            ExecuteMethod = command;
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
