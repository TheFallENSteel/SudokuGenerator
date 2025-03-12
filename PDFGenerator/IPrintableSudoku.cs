namespace PDFGenerator
{
    /// <summary>
    /// Interface of sudoku data types eligible to be printed.
    /// </summary>
    public interface IPrintableSudoku
    {
        /// <summary>
        /// Sudoku data in format of <see cref="int"/> array that represents numbers in sudoku.
        /// </summary>
        public int[] Data { get; }
        /// <summary>
        /// Size of sudoku square
        /// </summary>
        public int Size { get; }
    }
}
