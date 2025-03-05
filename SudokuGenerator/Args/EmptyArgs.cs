using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Args
{
    internal class EmptyArgs : CommandArgs
    {
        public static CommandArgsInfo CommandArgsInfo { get; } = new CommandArgsInfo([]);
        public override void Parse(List<string> args) { }
    }
}
