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
    public class PrintArgs
    {
        public int ColumnCount { get; }
        public int RowCount { get; }
        public string FileName;
        public string FileDirectory;
        public PageSize PageSize { get; }
        public PrintArgs(List<string> rawArgs)
        {
            rawArgs = rawArgs.Select(arg => arg.Trim().Trim('"').Trim('\'')).ToList();
            ColumnCount = 1;
            RowCount = 1;
            FileName = "OutputFile.pdf";
            FileDirectory = "";
            PageSize = PageSize.A4;
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
                RowCount = possibleRowCount >= 1 ? possibleRowCount : 1;
            }
            if (rawArgs.Count >= 5)
            {
                if (!int.TryParse(rawArgs[4], out int possibleColumnCount)) throw new ArgumentException("Invalid Column argument", nameof(ColumnCount));
                ColumnCount = possibleColumnCount >= 1 ? possibleColumnCount : 1;

            }
        }
    }
}
