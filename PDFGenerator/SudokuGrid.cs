using PDFGenerator;
using PdfSharp.Drawing;

namespace SudokuGenerator
{
    /// <summary>
    /// Provides methods to generate and draw Sudoku grids.
    /// </summary>
    public static class SudokuGrid
    {
        /// <summary>
        /// Prepares document pages for drawing the grid of Sudokus.
        /// </summary>
        /// <param name="sudoku">Array of Sudoku instances to be drawn.</param>
        /// <param name="offset">Offset to start drawing from the Sudoku array.</param>
        /// <param name="columnCount">Number of Sudokus in a row.</param>
        /// <param name="rowCount">Number of Sudokus in a column.</param>
        /// <param name="rectangle">Area for drawing the Sudokus.</param>
        /// <param name="spaceBetween">Gap between each Sudoku instance.</param>
        /// <returns>Returns an array of <see cref="PrintableSudokuDrawer"/> instances to be used for drawing.</returns>
        public static PrintableSudokuDrawer[] GenerateSudokuGrid(IPrintableSudoku[] sudoku, int offset, int columnCount, int rowCount, XPoint rectangle, XPoint spaceBetween)
        {
            PrintableSudokuDrawer[] returnValue = new PrintableSudokuDrawer[Math.Min(columnCount * rowCount, sudoku.Length - offset)];
            double size, centerOffsetX = 0, centerOffsetY = 0;
            double potentialX = (rectangle.X - columnCount * spaceBetween.X) / columnCount;
            double potentialY = (rectangle.Y - columnCount * spaceBetween.Y) / rowCount;
            if (potentialX < potentialY)
            {
                size = potentialX;
                centerOffsetY = (potentialY - size) / 2;
            }
            else
            {
                size = potentialY;
                centerOffsetX = (potentialX - size) / 2;
            }
            for (int i = 0; i < returnValue.Length; i++)
            {
                int column = i % columnCount;
                int row = i / columnCount;
                if (row >= rowCount) break;
                returnValue[i] = new PrintableSudokuDrawer(sudoku[i + offset],
                    new XPoint(
                        centerOffsetX + spaceBetween.X / 2 + column * (size + spaceBetween.X),
                        centerOffsetY + spaceBetween.Y / 2 + row * (size + spaceBetween.Y)),
                    new XPoint(size, size));
            }
            return returnValue;
        }
    }
}
