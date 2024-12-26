
using PdfSharp;
using PdfSharp.Charting;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Quality;
using PdfSharp.UniversalAccessibility.Drawing;
using SudokuGenerator;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace PDFGenerator
{
    /// <summary>
    /// Class that generates PDF File that represents the sudoku.
    /// <example>
    /// <code>
    /// //This is how you use the generator
    /// PDFGenerator generator = new PDFGenerator(filePath);
    /// generator.SaveSudokuDocument(settings, SomeSudokuDataClass, fileName);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="filePath"></param>
    public class PDFGenerator(string filePath)
    {
        public string FileDirectory { get; set; } = filePath;
        /// <summary>
        /// Saves one sudoku document to <see cref="FileDirectory"/>.
        /// </summary>
        /// <param name="settings">Object of <see cref="DocumentSettings"/> used to draw sudoku.</param>
        /// <param name="printableSudokus">Sudoku data to be printed.</param>
        /// <param name="fileName">Name of the generated file.</param>
        public void SaveSudokuDocument(DocumentSettings settings,
            IPrintableSudoku[] printableSudokus,
            string fileName)
        {
            PdfDocument doc = new PdfDocument();
            int sudokusPerPage = settings.RowCount * settings.ColumnCount;
            int pageCount = (printableSudokus.Length - 1) / sudokusPerPage + 1;
            for (int i = 0; i < pageCount; i++) 
            { 
                SaveSudokuPage(settings, printableSudokus, doc, i * sudokusPerPage);
            }
            if (FileDirectory != "") Directory.CreateDirectory(FileDirectory);
            doc.Save(Path.Combine(FileDirectory, fileName));
        }
        /// <summary>
        /// Saves one page of sudoku document
        /// </summary>
        /// <param name="settings">Object of <see cref="DocumentSettings"/> used to draw sudoku.</param>
        /// <param name="printableSudokus">Sudoku data to be printed.</param>
        /// <param name="doc">Current <see cref="PdfDocument"/> instance.</param>
        /// <param name="offset">Offset of the page.</param>
        protected static void SaveSudokuPage(DocumentSettings settings, IPrintableSudoku[] printableSudokus, PdfDocument doc, int offset)
        {
            PdfPage page = doc.AddPage();
            page.TrimMargins = settings.Margins;
            page.Size = settings.PageSize;

            IPrintableSudokuDrawer[] printableSudokuDrawers = SudokuGrid.GenerateSudokuGrid(
                printableSudokus, offset,
                settings.ColumnCount, settings.RowCount,
                new XPoint(page.Width.Point, page.Height.Point),
                settings.SudokuGap);
            XGraphics graphics = XGraphics.FromPdfPage(page);
            XStringFormat format = new XStringFormat()
            {
                LineAlignment = XLineAlignment.Center,
                Alignment = XStringAlignment.Center
            };
            for (int i = 0; i < printableSudokuDrawers.Length; i++)
            {
                DrawSudoku(graphics, settings, format, printableSudokuDrawers[i]);
            }
            DrawHeader(graphics, settings, format, page.Width.Point, $"Sudoku Generator");
        }
        /// <summary>
        /// Draws one sudoku.
        /// </summary>
        /// <param name="graphics">Object of <see cref="XGraphics"/> used to draw sudoku.</param>
        /// <param name="settings">Object of <see cref="DocumentSettings"/> used to draw sudoku.</param>
        /// <param name="sudokuDrawer">Object of <see cref="IPrintableSudokuDrawer"/> that is used to determine location of the square.</param>
        /// <param name="format">Object of <see cref="XStringFormat"/> used to draw sudoku.</param>
        protected static void DrawSudoku(XGraphics graphics, DocumentSettings settings, XStringFormat format, IPrintableSudokuDrawer sudokuDrawer)
        {
            double size = Math.Min(sudokuDrawer.RectSize.X, sudokuDrawer.RectSize.Y);
            double normalStep = (size / 9);
            DrawGrid(graphics, settings, sudokuDrawer, size, normalStep);
            DrawNumbers(graphics, settings, sudokuDrawer, normalStep, format);
        }
        /// <summary>
        /// Draws the sudoku grid.
        /// </summary>
        /// <param name="graphics">Object of <see cref="XGraphics"/> used to draw sudoku.</param>
        /// <param name="settings">Object of <see cref="DocumentSettings"/> used to draw sudoku.</param>
        /// <param name="sudokuDrawer">Object of <see cref="IPrintableSudokuDrawer"/> that is used to determine location of the square.</param>
        /// <param name="size">Size of the final sudoku area.</param>
        /// <param name = "step" > Step of the square that is being rendered.</param>
        protected static void DrawGrid(XGraphics graphics, DocumentSettings settings, IPrintableSudokuDrawer sudokuDrawer, double size, double step)
        {
            XPen gridPen = settings.GridMinorPen;
            double minorStep = step / 3;
            for (int i = 0; i < 28; i++)
            {
                if (i % 27 != 0 || i % 9 != 0 || i % 3 != 0)
                {
                    graphics.DrawLine(gridPen, new XPoint(sudokuDrawer.Offset.X, sudokuDrawer.Offset.Y + i * minorStep), new XPoint(sudokuDrawer.Offset.X + size, sudokuDrawer.Offset.Y + i * minorStep));
                    graphics.DrawLine(gridPen, new XPoint(sudokuDrawer.Offset.X + i * minorStep, sudokuDrawer.Offset.Y), new XPoint(sudokuDrawer.Offset.X + i * minorStep, sudokuDrawer.Offset.Y + size));
                }
            }
            DrawWhiteSpaces(graphics, sudokuDrawer, step);
            for (int i = 0; i < 10; i++)
            {
                if (i % 9 == 0) gridPen = settings.GridOuterPen;
                else if (i % 3 == 0) gridPen = settings.GridBoxDividerPen;
                else gridPen = settings.GridStandardPen;

                graphics.DrawLine(gridPen, new XPoint(sudokuDrawer.Offset.X, sudokuDrawer.Offset.Y + i * step), new XPoint(sudokuDrawer.Offset.X + size, sudokuDrawer.Offset.Y + i * step));
                graphics.DrawLine(gridPen, new XPoint(sudokuDrawer.Offset.X + i * step, sudokuDrawer.Offset.Y), new XPoint(sudokuDrawer.Offset.X + i * step, sudokuDrawer.Offset.Y + size));
            }
        }

        private static void DrawNumbers(XGraphics graphics, DocumentSettings settings, IPrintableSudokuDrawer sudokuDrawer, double step, XStringFormat format)
        {
            XFont numberFont = new XFont(settings.FontFamilyName, ((step / 2)));
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    DrawNumber(graphics, settings, sudokuDrawer, step, format, numberFont, i, j);
                }
            }
        }
        private static void DrawWhiteSpaces(XGraphics graphics, IPrintableSudokuDrawer sudokuDrawer, double step)
        {
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    DrawWhiteSpace(graphics, sudokuDrawer, step, i, j);
                }
            }
        }
        /// <summary>
        /// Draws string as header of the file.
        /// </summary>
        /// <param name="graphics">Object of <see cref="XGraphics"/> used to draw sudoku.</param>
        /// <param name="settings">Object of <see cref="DocumentSettings"/> used to draw sudoku.</param>
        /// <param name="format">Object of <see cref="XStringFormat"/> used to draw sudoku.</param>
        /// <param name="pageWidth">Width of the page.</param>
        /// <param name="headerString">String representing the header.</param>
        protected static void DrawHeader(XGraphics graphics, DocumentSettings settings, XStringFormat format, double pageWidth, string headerString)
        {
            XPoint location = new XPoint(pageWidth / 2, -15);
            graphics.DrawString(
                headerString,
                settings.HeaderFont, settings.FontBrush, location, format);
        }
        /// <summary>
        /// Draws square with correct number.
        /// </summary>
        /// <param name="graphics">Object of <see cref="XGraphics"/> used to draw sudoku.</param>
        /// <param name="settings">Object of <see cref="DocumentSettings"/> used to draw sudoku.</param>
        /// <param name="sudokuDrawer">Object of <see cref="IPrintableSudokuDrawer"/> that is used to determine location of the square.</param>
        /// <param name="format">Object of <see cref="XStringFormat"/> used to draw sudoku.</param>
        /// <param name="numberFont"><see cref="XFont"/> of the number to be rendered.</param>
        ///<param name = "step" > Step of the square that is being rendered.</param>
        /// <param name="i">Current X offset of the square.</param>
        /// <param name="j">Current Y offset of the square.</param>
        protected static void DrawNumber(XGraphics graphics, DocumentSettings settings, IPrintableSudokuDrawer sudokuDrawer, double step, XStringFormat format, XFont numberFont, int i, int j)
        {
            XPoint location = new XPoint(sudokuDrawer.Offset.X + i * step + step / 2, sudokuDrawer.Offset.Y + j * step + step / 2);
            int number = sudokuDrawer.Sudoku.Data[i + j * sudokuDrawer.Sudoku.Size];
            if (number != 0)
            {
                graphics.DrawString(
                    sudokuDrawer.Sudoku.Data[i + j * sudokuDrawer.Sudoku.Size].ToString(),
                    numberFont, settings.FontBrush, location, format);
            }
        }
        /// <summary>
        /// Draws white space to ensure good looking squares for printed spaces.
        /// </summary>
        /// <param name="graphics">Object of <see cref="XGraphics"/>used to draw sudoku.</param>
        /// <param name="sudokuDrawer">Object of <see cref="IPrintableSudokuDrawer"/> that is used to determine location of the square.</param>
        /// <param name="step">Step of the square that is being rendered.</param>
        /// <param name="i">Current X offset of the square.</param>
        /// <param name="j">Current Y offset of the square.</param>
        protected static void DrawWhiteSpace(XGraphics graphics, IPrintableSudokuDrawer sudokuDrawer, double step, int i, int j)
        {
            XPoint location = new XPoint(sudokuDrawer.Offset.X + i * step + step / 2, sudokuDrawer.Offset.Y + j * step + step / 2);
            int number = sudokuDrawer.Sudoku.Data[i + j * sudokuDrawer.Sudoku.Size];
            if (number != 0)
            {
                graphics.DrawRectangle(XBrushes.White, new XRect(location - new XVector(step / 2, step / 2), new XSize(step, step)));
            }
        }
    }
}
