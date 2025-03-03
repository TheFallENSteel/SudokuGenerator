using SudokuGenerator.Commands;
using SudokuSolver;
using System.Collections.Generic;

namespace SudokuGenerator
{
    public class ProgramState
    {
        public CommandsContainer Commands { get; } = new CommandsContainer();
        public List<Sudoku> SudokuBuffer { get; } = new List<Sudoku>();
        public Stack<Sudoku> SudokuSolutionBuffer { get; } = new Stack<Sudoku>();
        public bool Continue { get; set; }
    }
}