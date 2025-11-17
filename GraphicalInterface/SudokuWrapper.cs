using PDFGenerator;

namespace GraphicalInterface
{
    public class SudokuWrapper(SudokuData sudoku) : IPrintableSudoku
    {
        public SudokuData Sudoku { get; set; } = sudoku;
        public int[] Data { get => Sudoku.GetData(); }
        public int Size { get => 9; }
    }
}
