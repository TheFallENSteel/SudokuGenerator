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
        public override string ParametersHelp
        {
            get => "[SudokuString]";
        }
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
