using PdfSharp;
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
    public class GenerateArgs
    {
        public int GenerateCount { get; } = 6;
        public int Difficulty { get; } = Environment.TickCount;
        public GenerateArgs(List<string> rawArgs)
        {
            rawArgs = rawArgs.Select(arg => arg.Trim().Trim('"').Trim('\'')).ToList();
            if (rawArgs.Count >= 1)
            {
                if (!int.TryParse(rawArgs[0], out int difficulty)) throw new ArgumentException("Invalid Difficulty argument", nameof(Difficulty));
                Difficulty = difficulty;
            }
            if (rawArgs.Count >= 2) 
            { 
                if (!int.TryParse(rawArgs[1], out int generateCount)) throw new ArgumentException("Invalid GenerateCount argument", nameof(GenerateCount));
                GenerateCount = generateCount;
            }
        }
    }
}
