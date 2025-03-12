using PDFGenerator;
using SudokuGenerator.Args;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SudokuGenerator.Commands
{
    public static class SavePrintableFileCommand
    {
        public static readonly string[] Aliases = ["pdf", "export", "save", "s", "output", "out"];
        private const string Name = "pdf";
        private const string ShortDescription = "Creates pdf file for printing.";
        private const string LongDescription = "Saves all the sudokus from the buffer to be exported in pdf format.";
        public static CommandInfo CommandInfo => new CommandInfo(Name, ShortDescription, LongDescription, SavePrintableFileArgs.CommandArgsInfo);

        [ModuleInitializer]
        public static void Init()
        {
            Program.ProgramState.CommandContainer.AddCommand(new Command(Execute, CommandInfo, Aliases));
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
