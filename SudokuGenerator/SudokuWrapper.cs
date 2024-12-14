using PDFGenerator;
using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator
{
    public class SudokuWrapper(Sudoku sudoku) : IPrintableSudoku
    {
        public Sudoku Sudoku { get; set; } = sudoku;
        public int[] Data { get => Sudoku.GetData(); }
        public int Size { get => 9; }
    }
}
