using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Args
{
    public class SolveSudokuArgs : CommandArgs
    {
        public int Difficulty { get; private set; }
        public override string ParametersHelp
        {
            get => "[MaxDifficulty]";
        }
        public override void Parse(List<string> rawArgs)
        {
            this.Difficulty = int.MaxValue;

            rawArgs = rawArgs.Select(arg => arg.Trim().Trim('"').Trim('\'')).ToList();
            if (rawArgs.Count >= 1)
            {
                if (!int.TryParse(rawArgs[0], out int difficulty)) throw new ArgumentException("Invalid Difficulty argument", nameof(Difficulty));
                this.Difficulty = difficulty;
            }
        }
    }
}
