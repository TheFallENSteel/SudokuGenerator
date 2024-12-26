using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFGenerator
{
    /// <summary>
    /// Data for drawing sudokus.
    /// </summary>
    public class IPrintableSudokuDrawer(IPrintableSudoku sudoku, XPoint offset, XPoint rectSize)
    {
        public IPrintableSudoku Sudoku { get; } = sudoku;
        public XPoint Offset { get; set; } = offset;
        public XPoint RectSize { get; } = rectSize;
    }
}
