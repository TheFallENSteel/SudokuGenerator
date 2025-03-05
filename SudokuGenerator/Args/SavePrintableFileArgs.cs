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
        public PageSize PageSize { get; private set; } = PageSize.A4;
        public int ColumnCount { get; private set; } = 1;
        public int RowCount { get; private set; } = 1;

        public string FileName { get; private set; } = "OutputFile.pdf";
        public string FileDirectory { get; private set; } = Directory.GetCurrentDirectory();

        public static CommandArgsInfo CommandArgsInfo { get; } = new CommandArgsInfo([
            new ParameterInfo("[FileName]", "string", "The name of the file to save. Default is \"OutputFile.pdf\"."),
            new ParameterInfo("[FileDirectory]", "string path", "The directory to save the file in. Default is current directory."),
            new ParameterInfo("[PageSize]", "page printing format", "The size format of the document that you want to use ex. A4, Letter... Default is A4."),
            new ParameterInfo("[RowCount]", "positive integer", "The number of rows of sudokus on the final page. Default is 1."),
            new ParameterInfo("[ColumnCount]", "positive integer", "The number of columns of sudokus on the final page. Default is 1.")
        ]);

        public override void Parse(List<string> rawArgs)
        {

            rawArgs = rawArgs.Select(arg => arg.Trim().Trim('"').Trim('\'')).ToList();
            if (rawArgs.Count >= 1)
            {
                FileName = rawArgs[0];
            }
            if (rawArgs.Count >= 2)
            {
                FileDirectory = rawArgs[1];
            }
            if (rawArgs.Count >= 3)
            {
                if (!Enum.TryParse(typeof(PageSize), rawArgs[2], true, out object? possiblePageSize)) throw new ArgumentException("Invalid PageSize argument", nameof(PageSize));
                PageSize = (PageSize)possiblePageSize;
            }
            if (rawArgs.Count >= 4)
            {
                if (!int.TryParse(rawArgs[3], out int possibleRowCount)) throw new ArgumentException("Invalid RowCount argument", nameof(RowCount));
                RowCount = possibleRowCount > 1 ? possibleRowCount : 1;
            }
            if (rawArgs.Count >= 5)
            {
                if (!int.TryParse(rawArgs[4], out int possibleColumnCount)) throw new ArgumentException("Invalid Column argument", nameof(ColumnCount));
                ColumnCount = possibleColumnCount > 1 ? possibleColumnCount : 1;
            }
        }
    }
}
