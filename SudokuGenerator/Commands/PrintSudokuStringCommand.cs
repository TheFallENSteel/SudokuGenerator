using SudokuGenerator.Args;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator.Commands
{
    public static class PrintSudokuStringCommand
    {
        static PrintSudokuStringCommand()
        {
            Program.ProgramState.Commands.AddCommand(new Command(Execute, ["print", "pr", "data", "d"]));
        }
        public static string? Execute(List<string> rawArgs, out bool success)
        {
            PrintSudokuStringArgs args = new PrintSudokuStringArgs();
            args.Parse(rawArgs);
            string returnValue = "";
            if (Program.ProgramState.SudokuBuffer.Count == 0) 
            { 
                success = false;
                return "Sudoku buffer is empty!";
            }
            for (int i = 0; i < Program.ProgramState.SudokuBuffer.Count; i++)
            {
                if (args.Raw) returnValue += $"Sudoku {i + 1}: \n{Program.ProgramState.SudokuBuffer[i].ToString()} \n";
                else returnValue += $"Sudoku {i + 1}: \n{Program.ProgramState.SudokuBuffer[i].ToFormattedString()} \n";
            }
            success = true;
            return "Sudoku Buffer: \n" + returnValue;
        }
    }
}
