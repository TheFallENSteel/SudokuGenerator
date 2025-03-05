using PdfSharp;
using SudokuGenerator.Args;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator
{
    public class GenerateArgs : CommandArgs
    {
        public int GenerateCount { get; private set; } = 1;
        public int Difficulty { get; private set; } = int.MaxValue;
        public static CommandArgsInfo CommandArgsInfo { get; } = new CommandArgsInfo([
                new ParameterInfo(
                    "Difficulty",
                    "int",
                    "Determines the maximum difficulty of the generated sudoku. "
                    +$"\n<1: Disallows all solving methods" +
                    $"\n>={Sudoku.MAX_DIFFICULTY}: Allows all solving methods" +
                    "\nDefault value is 1."),
                new ParameterInfo(
                    "Count",
                    "positive int",
                    "Determines how many sudokus should be generated. " +
                    "\nDefault value is 1.")
            ]);
            //get => ["[Difficulty]", "[Count]"];
        public override void Parse(List<string> rawArgs)
        {

            rawArgs = rawArgs.Select(arg => arg.Trim().Trim('"').Trim('\'')).ToList();
            if (rawArgs.Count >= 1)
            {
                if (!int.TryParse(rawArgs[0], out int difficulty)) throw new ArgumentException("Invalid Difficulty argument", nameof(Difficulty));
                this.Difficulty = difficulty;
            }
            if (rawArgs.Count >= 2) 
            { 
                if (!int.TryParse(rawArgs[1], out int generateCount)) throw new ArgumentException("Invalid GenerateCount argument", nameof(GenerateCount));
                GenerateCount = generateCount;
            }
        }
    }
}
