using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            rawArgs = rawArgs.Select(arg => arg.Trim().Trim('"').Trim('\'')).ToList();
            if (rawArgs.Count >= 1)
            {
                this.SudokuString = rawArgs[0];
            }
        }
    }
}
