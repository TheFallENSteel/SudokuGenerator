using Microsoft.VisualBasic;
using PDFGenerator;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator
{
    public static class SudokuGrid
    {
        /// <summary>
        /// Preparing document pages for drawing the grid of sudokus.
        /// </summary>
        /// <param name="sudoku"></param>
        /// <param name="offset"></param>
        /// <param name="columnCount">Number of sudokus in row.</param>
        /// <param name="rowCount">Number of sudokus in column.</param>
        /// <param name="rectangle">Area for the drawing of sudokus.</param>
        /// <param name="spaceBetween">Gap between each sudoku instances.</param>
        /// <returns>Returns <see cref="IPrintableSudokuDrawer"/> instance to be used for drawing.</returns>
        public static IPrintableSudokuDrawer[] GenerateSudokuGrid(IPrintableSudoku[] sudoku, int offset, int columnCount, int rowCount, XPoint rectangle, XPoint spaceBetween) 
        {
            IPrintableSudokuDrawer[] returnValue = new IPrintableSudokuDrawer[Math.Min(columnCount * rowCount, sudoku.Length - offset)];
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
                returnValue[i] = new IPrintableSudokuDrawer(sudoku[i + offset], 
                    new XPoint(
                        centerOffsetX + spaceBetween.X / 2 + column * (size + spaceBetween.X),
                        centerOffsetY + spaceBetween.Y / 2 + row * (size + spaceBetween.Y)),
                    new XPoint(size, size));
            }
            return returnValue;
        }
    }
}
