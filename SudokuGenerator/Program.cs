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
        static List<Sudoku> SudokuBuffer { get; } = new List<Sudoku>();
        static Stack<Sudoku> SudokuSolutionBuffer { get; } = new Stack<Sudoku>();
        static void Main()
        {
            bool toBeContinued = true;
            Console.WriteLine("SudokuCenter 1.0 by Martin Komárek\nUse h or help for list of commands:");
            while (toBeContinued)
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
                    switch (command)
                    {
                        case "add" or "+":
                            AddSudoku(args, SudokuBuffer);
                            break;
                        case "cl" or "clear":
                            SudokuBuffer.Clear();
                            break;
                        case "g" or "gen" or "generate":
                            SudokuBuffer.AddRange(Generate(args));
                            break;
                        case "e" or "exit":
                            toBeContinued = false;
                            break;
                        case "save":
                            try 
                            { 
                                if (SudokuBuffer.Count == 0) break;
                                SaveSudoku(SudokuBuffer, args);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Could not print file, error message: {e.Message}");
                            }
                            break;
                        case "p" or "print":
                            SudokuBuffer.ForEach(s => Console.WriteLine(s.ToFormattedString()));
                            break;
                        case "solve":
                            try
                            {
                                foreach(Sudoku sudoku in SudokuBuffer) 
                                { 
                                    SolveAndPrintResults(args, sudoku);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Sudoku Error: {e.Message}");
                            }
                            break;
                        case "d" or "data":
                            if (SudokuBuffer.Count == 0) break;
                            for (int i = 0; i < SudokuBuffer.Count; i++)
                            {
                                Console.WriteLine($"Sudoku {i + 1}:\n{SudokuBuffer[i].ToString()}");
                            }
                            break;
                        case "h" or "help":
                            //TODO: Implement
                            break;
                    }
                }
                catch (ArgumentException e) 
                {
                    Console.WriteLine($"Invalid parameter: {e.ParamName}\nin command {command}");
                }
                catch (Exception e) 
                { 
                    Console.WriteLine($"Invalid operation: {e.Message}");
                }
            }
        }
        private static void GenerateSolutions(int count)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    Sudoku solution = GenerateSolution();
                    lock (SudokuSolutionBuffer)
                    {
                        SudokuSolutionBuffer.Push(solution);
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
                if (SudokuSolutionBuffer.Count == 0)
                {
                    SudokuSolutionBuffer.Push(Sudoku.GenerateRandomSolved());
                }
                solution = SudokuSolutionBuffer.Pop();
                solution.UnSolve(difficulty);
                returnValue.Add(solution);
            }
            return returnValue;
        }

        private static void AddSudoku(List<string> args, List<Sudoku> sudokuBuffer) 
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
        }

        private static Sudoku SolveAndPrintResults(List<string> args, Sudoku sudoku)
        {
            if (args.Count == 0) args.Add(int.MaxValue.ToString());
            Sudoku? result = sudoku.Solve(int.Parse(args[0]));
            Console.WriteLine(result?.ToFormattedString());
            return sudoku;
        }

        private static void SaveSudoku(List<Sudoku> sudokuToPrint, List<string> rawArgs)
        {
            PrintArgs args = new PrintArgs(rawArgs);
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
        //000060030720000004096704001000090060109008403640005000005209300960007005007800109
        //000060030720000004096704001000090060109008403640005000005209300960007005007800009
    }
}
