using PDFGenerator;
using SudokuGenerator.Args;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SudokuGenerator.Commands
{
    public static class SavePrintableFileCommand
    {
        static SavePrintableFileCommand() 
        {
            Program.ProgramState.Commands.AddCommand(new Command(Execute, ["pdf", "save", "s", "output", "out"]));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            success = false;
            SavePrintableFileArgs args = new SavePrintableFileArgs();
            args.Parse(rawArgs);
            try
            {
                if (Program.ProgramState.SudokuBuffer.Count == 0) return "No Sudoku was printed. You must first generate one with 'generate' command";
                SaveSudoku(args, Program.ProgramState.SudokuBuffer);
            }
            catch (Exception e)
            {
                return $"Could not print file, error message: {e.Message}";
            }
            success = true;
            return $"File {args.FileName} in directory: {args.FileDirectory} was created successfully";
        }
        private static void SaveSudoku(SavePrintableFileArgs args, List<Sudoku> sudokuToPrint)
        {
            PDFGenerator.PDFGenerator generator = new PDFGenerator.PDFGenerator(args.FileDirectory);
            generator.SaveSudokuDocument(
                new DocumentSettings(
                    args.RowCount,
                    args.ColumnCount,
                    args.PageSize),
                sudokuToPrint.Select(sud => new SudokuWrapper(sud)).ToArray(),
                args.FileName);
            new Process
            {
                StartInfo = new ProcessStartInfo(args.FileName)
                {
                    UseShellExecute = true,
                    WorkingDirectory = args.FileDirectory
                }
            }.Start();
        }
    }
}
