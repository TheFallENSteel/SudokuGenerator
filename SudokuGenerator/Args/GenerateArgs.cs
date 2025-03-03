using PdfSharp;
using SudokuGenerator.Args;
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
        public int Difficulty { get; private set; } = Environment.TickCount;
        public override string ParametersHelp 
        {
            get => "[Difficulty] [Count]"; 
        }
        public override void Parse(List<string> rawArgs)
        {
            this.Difficulty = 1;
            GenerateCount = 1;

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
