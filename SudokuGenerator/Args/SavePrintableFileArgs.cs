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
    public class SavePrintableFileArgs : CommandArgs
    {
        public PageSize PageSize { get; private set; }
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }
        public string FileName { get; private set; }
        public string FileDirectory { get; private set; }

        public static CommandArgsInfo CommandArgsInfo { get; } = new CommandArgsInfo([
            new ParameterInfo("[FileName]", "string", "The name of the file to save. Default is \"OutputFile.pdf\"."),
            new ParameterInfo("[FileDirectory]", "string path", "The directory to save the file in. Default is current directory."),
            new ParameterInfo("[PageSize]", "page printing format", "The size format of the document that you want to use ex. A4, Letter... Default is A4."),
            new ParameterInfo("[RowCount]", "positive integer", "The number of rows of sudokus on the final page. Default is 1."),
            new ParameterInfo("[ColumnCount]", "positive integer", "The number of columns of sudokus on the final page. Default is 1.")
        ]);

        public override void Parse(List<string> rawArgs)
        {
            rawArgs = CommandArgs.ProcessArgs(rawArgs);
            this.FileName = CommandArgs.ParseArg(rawArgs, 0, "OutputFile.pdf");
            this.FileDirectory = CommandArgs.ParseArg(rawArgs, 1, Directory.GetCurrentDirectory());
            this.PageSize = CommandArgs.ParseArg<PageSize>(rawArgs, 2, PageSize.A4);
            this.RowCount = CommandArgs.ParseArg(rawArgs, 3, 1);
            this.ColumnCount = CommandArgs.ParseArg(rawArgs, 4, 1);
        }
    }
}
