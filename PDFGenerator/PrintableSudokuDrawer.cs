using PdfSharp.Drawing;

namespace PDFGenerator
{
    /// <summary>
    /// Data for drawing sudokus.
    /// </summary>
    public class PrintableSudokuDrawer(IPrintableSudoku sudoku, XPoint offset, XPoint rectSize)
    {
        public IPrintableSudoku Sudoku { get; } = sudoku;
        public XPoint Offset { get; set; } = offset;
        public XPoint RectSize { get; } = rectSize;
    }
}
