using PdfSharp;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Args
{
    public class PrintSudokuStringArgs : CommandArgs
    {
        public bool Raw { get; private set; }

        public static CommandArgsInfo CommandArgsInfoget{get; } = new CommandArgsInfo([
                new ParameterInfo(
                    "Raw",
                    "bool",
                    "Determines if sudoku string should be printed in raw format, " +
                    "\nthat is without spaces and new lines." +
                    "Default value is false.")
            ]);

        public override void Parse(List<string> rawArgs)
        {
            rawArgs = CommandArgs.ProcessArgs(rawArgs);
            this.Raw = CommandArgs.ParseArg(rawArgs, 0, false);
        }
    }
}
