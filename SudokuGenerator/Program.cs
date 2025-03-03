using System;
using SudokuSolver;
using PDFGenerator;
using System.IO;
using SudokuGenerator.Args;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;


namespace SudokuGenerator
{
    public class Program
    {
        public static ProgramState ProgramState { get; } = new ProgramState();
        static void Main()
        {
            Console.WriteLine("SudokuCenter 1.0 by Martin Komárek\nUse h or help for list of commands:");
            while (ProgramState.Continue)
            {
                Console.WriteLine("Enter your command:");
                string command = "";
                try
                {
                    string? fullCommandRaw = Console.ReadLine();
                    if (fullCommandRaw == null) break;
                    List<string> fullCommand = fullCommandRaw.Split().Select(com => com.ToLower()).ToList();
                    command = fullCommand[0];
                    fullCommand.RemoveAt(0);
                    List<string> args = fullCommand;
                }
                catch (ArgumentException e) 
                {
                    Console.WriteLine($"Invalid parameter: '{e.ParamName}' \nin command: '{command}'");
                }
                catch (Exception e) 
                { 
                    Console.WriteLine($"Invalid operation: '{e.Message}'");
                }
            }
        }
        /*private static void GenerateSolutions(int count)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    Sudoku solution = GenerateSolution();
                    lock (ProgramState.SudokuSolutionBuffer)
                    {
                        ProgramState.SudokuSolutionBuffer.Push(solution);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
        }

        private static Sudoku GenerateSolution()
        {
            return Sudoku.GenerateRandomSolved();
        }

        private static List<Sudoku> Generate(List<string> rawArgs)
        {
            GenerateArgs args = new GenerateArgs(rawArgs);
            List<Sudoku> returnValue = new List<Sudoku>(args.GenerateCount);
            int difficulty = args.Difficulty;
            GenerateSolutions(args.GenerateCount);
            for (int i = 0; i < args.GenerateCount; i++)
            {
                Sudoku solution;
                if (ProgramState.SudokuSolutionBuffer.Count == 0)
                {
                    ProgramState.SudokuSolutionBuffer.Push(Sudoku.GenerateRandomSolved());
                }
                solution = ProgramState.SudokuSolutionBuffer.Pop();
                solution.UnSolve(difficulty);
                returnValue.Add(solution);
            }
            return returnValue;
        }
        */

        /*private static void AddSudoku(List<string> args, List<Sudoku> sudokuBuffer) 
        { 
            string? rawSudoku = "";
            if (args.Count == 0) 
            { 
                Console.WriteLine("Input your sudoku in format or Exit with e:\n000 102 378 000...\n[Ignores white spaces]");
                rawSudoku = Console.ReadLine();
                if (rawSudoku == null) return;
            }
            else
            {
                rawSudoku = args[0];
            }
            int[] sudokuData = SudokuLoader.LoadFromString(rawSudoku, null);
            sudokuBuffer.Add(new Sudoku(sudokuData));
        }*/

        /*private static Sudoku SolveAndPrintResults(List<string> args, Sudoku sudoku)
        {
            if (args.Count == 0) args.Add(int.MaxValue.ToString());
            Sudoku? result = sudoku.Solve(int.Parse(args[0]));
            Console.WriteLine(result?.ToFormattedString());
            return sudoku;
        }*/

        /*private static void SaveSudoku(List<Sudoku> sudokuToPrint, List<string> rawArgs)
        {
            SavePrintableFileArgs args = new PrintArgs(rawArgs);
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
        }*/
        //000060030720000004096704001000090060109008403640005000005209300960007005007800109
        //000060030720000004096704001000090060109008403640005000005209300960007005007800009
    }
}
