using System.Collections.Generic;

namespace SudokuGenerator.Args
{
    public class AddSudokuArgs : CommandArgs
    {
        public string? SudokuString { get; private set; }
        public static CommandArgsInfo CommandArgsInfo { get; } = new CommandArgsInfo(
        [
            new ParameterInfo(
                "SudokuString", "string", "The sudoku string representing the sudoku. " +
                "\nIt is in format 000 102 378 000..." +
                "\n[Ignores white spaces]")
        ]
    );
        public override void Parse(List<string> rawArgs)
        {
            rawArgs = CommandArgs.ProcessArgs(rawArgs);
            this.SudokuString = CommandArgs.ParseArg(rawArgs, 0, "");
        }
    }
}
