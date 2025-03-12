using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PDFGenerator
{
    public class DocumentSettings(int rowCount, int columnCount, TrimMargins margins, XPoint sudokuGap, string fontFamilyName, XPen gridMinorPen, XPen gridStandardPen, XPen gridBoxDividerPen, XPen gridOuterBrush, XBrush fontBrush, PageSize pageSize = PageSize.A4, string name = "OutputFile.pdf")
    {
        public DocumentSettings(int rowCount, int columnCount, PageSize pageSize = PageSize.A4, string name = "OutputFile.pdf") :
            this(rowCount, columnCount, new TrimMargins() { All = new XUnit(25), Top = new XUnit(50) }, new XPoint(5, 5), "Arial", new XPen(XPens.LightGray) { Width = 0.25, LineCap = XLineCap.Square }, new XPen(XPens.Black) { Width = 0.5, LineCap = XLineCap.Square }, new XPen(XPens.Black) { Width = 1.5, LineCap = XLineCap.Square }, new XPen(XPens.Black) { Width = 2.5, LineCap = XLineCap.Square }, XBrushes.Black, pageSize, name)
        { }

        public XPen GridMinorPen { get; set; } = gridMinorPen;
        public XPen GridStandardPen { get; set; } = gridStandardPen;
        public XPen GridOuterPen { get; set; } = gridOuterBrush;
        public XPen GridBoxDividerPen { get; set; } = gridBoxDividerPen;
        public XBrush FontBrush { get; set; } = fontBrush;
        public string FontFamilyName { get; set; } = fontFamilyName;
        public int RowCount { get; set; } = rowCount;
        public int ColumnCount { get; set; } = columnCount;
        public TrimMargins Margins { get; set; } = margins;
        public PageSize PageSize { get; set; } = pageSize;
        public string Name { get; set; } = name;
        public XPoint SudokuGap { get; set; } = sudokuGap;

        public int Seed { get; set; } = 0;
        public XFont HeaderFont { get; set; } = new XFont(fontFamilyName, 25);
        public string Difficulty { get; set; } = "Difficulty";
    }
}
