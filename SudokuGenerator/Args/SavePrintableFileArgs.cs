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
        public int ColumnCount { get; private set; }
        public PageSize PageSize { get; private set; }
        public int RowCount { get; private set; }

        public string FileName;
        public string FileDirectory;

        public override string ParametersHelp
        {
            get => "[FileName] [FileDirectory] [PageSize] [RowCount] [ColumnCount]";
        }

        public override void Parse(List<string> rawArgs)
        {
            ColumnCount = 1;
            RowCount = 1;
            FileName = "OutputFile.pdf";
            FileDirectory = Directory.GetCurrentDirectory();
            PageSize = PageSize.A4;

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
