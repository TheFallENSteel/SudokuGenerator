using System.Collections.Generic;

namespace SudokuGenerator.Args
{
    internal class EmptyArgs : CommandArgs
    {
        public static CommandArgsInfo CommandArgsInfo { get; } = new CommandArgsInfo([]);
        public override void Parse(List<string> args) { }
    }
}
