using SudokuGenerator.Args;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public static class AddSudokuCommand
    {
        static AddSudokuCommand() 
        {
            Program.ProgramState.Commands.AddCommand(new Command(Execute, "", AddSudokuArgs.ParametersHelp, ["add", "+", "append"]));
        }
        public static string? Execute(List<string> rawArgs, out bool success) 
        {
            AddSudokuArgs args = new AddSudokuArgs();
            success = false;
            args.Parse(rawArgs);
            if (args.SudokuString == null) return null;
            try
            {
                int[] sudokuData = SudokuLoader.LoadFromString(args.SudokuString, null);
                Program.ProgramState.SudokuBuffer.Add(new Sudoku(sudokuData));
                success = true;
                return $"Sudoku: {args.SudokuString}, was added successfully";
            }
            catch 
            {
                return $"String was not in valid format. \nThe correct format is: \n000060030720000... ";
            }
        }
    }
}
