using PDFGenerator;
using SudokuSolver;

namespace SudokuGenerator
{
    public class SudokuWrapper(Sudoku sudoku) : IPrintableSudoku
    {
        public Sudoku Sudoku { get; set; } = sudoku;
        public int[] Data { get => Sudoku.GetData(); }
        public int Size { get => 9; }
    }
}
