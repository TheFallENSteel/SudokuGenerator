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

        public override string ParametersHelp
        {
            get => "[Raw]";
        }

        public override void Parse(List<string> rawArgs)
        {
            this.Raw = false;

            rawArgs = rawArgs.Select(arg => arg.Trim().Trim('"').Trim('\'').ToLower()).ToList();
            if (rawArgs.Count >= 1)
            {
                switch (rawArgs[0]) 
                { 
                    case "r": case "raw": case "true": case "t": case "1":
                        this.Raw = true;
                    break;
                    default:
                        this.Raw = false;
                        break;
                }
            }
        }
    }
}
