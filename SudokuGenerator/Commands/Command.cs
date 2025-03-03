using SudokuGenerator.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public class Command : ICommand
    {
        public delegate string? ExecuteCommand(List<string> args, out bool success);
        public ExecuteCommand ExecuteMethod { get; }
        public string[] Aliases { get; }
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

        public string? Execute(List<string> commandArgs, int index, out bool success)
        {
            if (index == 0) 
            { 
                commandArgs.RemoveAt(0);
                return Execute(commandArgs, out success);
            }
            success = false;
            return null;
        }
    }
}
