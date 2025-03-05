using SudokuGenerator.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public class PositionalCommand : Command
    {
        public delegate string? ExecutePositionalCommandCommand(List<string> args, out bool success);
        private ExecutePositionalCommandCommand ExecutePositionalMethod { get; }
        public PositionalCommand(ExecutePositionalCommandCommand positionalCommand, ParameterInfo parameterInfo, ExecuteCommand command, string[] aliases) : base(command, aliases: parameterInfo, helpString: aliases)
        {
            ExecutePositionalMethod = positionalCommand;
        }
        public virtual string? ExecutePositional(List<string> args, out bool success)
        {
            return ExecutePositionalMethod.Invoke(args, out success);
        }
    }
}
