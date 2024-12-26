
using PdfSharp;
using PdfSharp.Charting;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Quality;
using PdfSharp.UniversalAccessibility.Drawing;
using SudokuGenerator;
using System.Diagnostics;

namespace PDFGenerator
{
    public class PDFGenerator(string filePath)
    {
        public string FileDirectory { get; set; } = filePath;

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

        private static void SaveSudokuPage(DocumentSettings settings, IPrintableSudoku[] printableSudokus, PdfDocument doc, int offset)
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
            DrawHeader(graphics, settings, settings.HeaderFont, format, page.Width.Point);
        }

        private static void DrawSudoku(XGraphics graphics, DocumentSettings settings, XStringFormat format, IPrintableSudokuDrawer sudokuDrawer)
        {
            double size = Math.Min(sudokuDrawer.RectSize.X, sudokuDrawer.RectSize.Y);
            double normalStep = (size / 9);
            DrawGrid(graphics, settings, sudokuDrawer, size, normalStep);
            DrawNumbers(graphics, settings, sudokuDrawer, normalStep, format);
        }


        private static void DrawGrid(XGraphics graphics, DocumentSettings settings, IPrintableSudokuDrawer sudokuDrawer, double size, double step)
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
                else gridPen = settings.GridRegularPen;

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

        private static void DrawHeader(XGraphics graphics, DocumentSettings settings, XFont font, XStringFormat format, double PageWidth)
        {
            XPoint location = new XPoint(PageWidth / 2, -15);
            graphics.DrawString(
                $"Difficulty:{settings.Difficulty} Seed:{settings.Seed}",
                settings.HeaderFont, settings.FontBrush, location, format);
        }

        private static void DrawNumber(XGraphics graphics, DocumentSettings settings, IPrintableSudokuDrawer sudokuDrawer, double step, XStringFormat format, XFont numberFont, int i, int j)
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
        private static void DrawWhiteSpace(XGraphics graphics, IPrintableSudokuDrawer sudokuDrawer, double step, int i, int j)
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
