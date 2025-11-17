using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SudokuSolver
{
    public class SudokuData
    {
        private Container[] RowContainers = new Container[9];
        private Container[] ColumnContainers = new Container[9];
        private Container[] SquareContainers = new Container[9];
        public Cell[] Cells = new Cell[81];

        public SudokuData()
        {
            GenerateGrid();
        }
        public SudokuData(int[] data, bool toBeValidated = false)
        {
            GenerateGrid();
            if (!toBeValidated && !SetData(data))
            {
                throw new InvalidDataException("Invalid Sudoku data");
            }
        }

        public static int Size { get => 9; }
        public static int SquareSize { get => 3; }

        public bool SetData(int[] data)
        {
            ValidateSize(data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                ValidateValue(data[i]);
                if (!Cells[i].TrySetValue(data[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public int[] GetRawData()
        {
            return Cells.Select(cell => cell.Value).ToArray();
        }

        public bool CheckForNakedValues()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i].ConfirmDefinitiveValue())
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckForHiddenValues()
        {
            foreach (var row in RowContainers)
            {
                if (row.CheckForHiddenValues()) return true;
            }
            foreach (var col in ColumnContainers)
            {
                if (col.CheckForHiddenValues()) return true;
            }
            foreach (var sqr in SquareContainers)
            {
                if (sqr.CheckForHiddenValues()) return true;
            }
            return false;
        }

        public bool CheckForPointing(Container[] mainContainers, Container[] secondaryContainers)
        {
            bool returnValue = false;
            foreach (var primaryContainer in mainContainers)
            {
                foreach (var secondaryContainer in secondaryContainers)
                {
                    List<int> intersectionPossibilities = Cell.CellsPossibilities(primaryContainer.Intersection(secondaryContainer));
                    List<int> exclusionPossibilities = Cell.CellsPossibilities(primaryContainer.Exclusion(secondaryContainer));
                    List<int> pointingPossibilities = intersectionPossibilities.Except(exclusionPossibilities).ToList();
                    foreach (var possibility in pointingPossibilities)
                    {
                        List<Cell> otherCells = secondaryContainer.Exclusion(primaryContainer).Where(cell => cell.IsPossible(possibility)).ToList();
                        if (otherCells.Count > 0)
                        {
                            otherCells.ForEach(cell => cell.RemovePossibility(possibility));
                            returnValue = true;
                        }
                    }
                }
            }
            return returnValue;
        }

        public bool CheckForPointing()
        {
            return CheckForPointing(SquareContainers, ColumnContainers.Concat(RowContainers).ToArray());
        }

        public bool CheckForClaiming()
        {
            return CheckForPointing(ColumnContainers.Concat(RowContainers).ToArray(), SquareContainers);
        }

        private static int ValidateValue(int data)
        {
            if (data < 0 || data > Size) throw new Exception("Invalid number in sudoku");
            else return data;
        }
        private static void ValidateSize(int dataLength)
        {
            if (dataLength != Size * Size) throw new Exception("Invalid size of sudoku");
        }

        private void GenerateGrid()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new Cell(i % Size, i / Size);
            }
            for (int i = 0; i < 9; i++)
            {
                RowContainers[i] = new Container(GetRowCells(i), i, "Row");
                ColumnContainers[i] = new Container(GetColumnCells(i), i, "Column");
                SquareContainers[i] = new Container(GetSquareCells(i % SquareSize, i / SquareSize), i, "Square");
            }
        }
        public Container GetRow(int row) => RowContainers[row];
        public Container GetColumn(int column) => RowContainers[column];
        public Container GetSquare(int squareX, int squareY) => RowContainers[squareX + squareY * SquareSize];
        public Cell GetCell(int cellX, int cellY) => Cells[cellX + cellY * Size];
        private static int GetCellIndex(int cellX, int cellY) => cellX + cellY * Size;
        public bool IsSolved()
        {
            return RowContainers.All(row => row.IsSolved())
                && ColumnContainers.All(col => col.IsSolved())
                && SquareContainers.All(sqr => sqr.IsSolved());
        }
        public int GetEmptyCellsCount()
        {
            int counter = 0;
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i].Value == 0) counter++;
            }
            return counter;
        }
        private Cell[] GetRowCells(int row)
        {
            return Enumerable.Range(0, Size).Select(c => GetCell(c, row)).ToArray();
        }
        private Cell[] GetColumnCells(int column)
        {
            return Enumerable.Range(0, Size).Select(r => GetCell(column, r)).ToArray();
        }
        private Cell[] GetSquareCells(int squareX, int squareY)
        {
            return Enumerable.Range(0, Size).Select(n => GetCell(squareX * SquareSize + n % SquareSize, squareY * SquareSize + n / SquareSize)).ToArray();
        }

        public Cell? LeastVariableUnsetCell()
        {
            Cell? returnValue = null;
            for (int i = 1; i < Cells.Length; i++)
            {
                if (Cells[i].Possibilities.Count > 1 && (returnValue == null || (Cells[i].Possibilities.Count < returnValue.Possibilities.Count))) returnValue = Cells[i];
            }
            return returnValue;
        }

        public string ToFormattedString()
        {
            string returnValue = "";
            for (int j = 0; j < Size; j++)
            {
                for (int i = 0; i < Size; i++)
                {
                    returnValue += GetCell(i, j).Value + " ";
                }
                returnValue += "\n";
            }
            return returnValue;
        }
        public override string ToString()
        {
            string returnValue = "";
            for (int j = 0; j < Size; j++)
            {
                for (int i = 0; i < Size; i++)
                {
                    returnValue += GetCell(i, j).Value;
                }
            }
            return returnValue;
        }

        public static int[] FlipData(int[] data)
        {
            int[] flippedData = new int[Size * Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    flippedData[GetCellIndex(i, j)] = data[GetCellIndex(Size - i - 1, Size - j - 1)];
                }
            }
            return flippedData;
        }
        public static int[] Mirror(int[] data, bool xAxis)
        {
            int[] flippedData = new int[Size * Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    flippedData[GetCellIndex(i, j)] = xAxis ? data[GetCellIndex(Size - i - 1, j)] : data[GetCellIndex(i, Size - j - 1)];
                }
            }
            return flippedData;
        }
        public static int[] SwapNumbers(int[] data, int[] dictionary) //Nine numbers
        {
            int[] flippedData = new int[Size * Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (data[GetCellIndex(i, j)] != 0) flippedData[GetCellIndex(i, j)] = dictionary[data[GetCellIndex(i, j)] - 1];
                }
            }
            return flippedData;
        }
        public static int[] SwapRowsColumns(int[] data, int[] dictionaryRows, int[] dictionaryColumns) //Three numbers
        {
            int[] flippedData = new int[Size * Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    int xIndex = dictionaryRows[(i / 3)] * 3 + (i % 3);
                    int yIndex = dictionaryColumns[(j / 3)] * 3 + (j % 3);
                    flippedData[GetCellIndex(i, j)] = data[GetCellIndex(xIndex, yIndex)];
                }
            }
            return flippedData;
        }
        public static int[] RandomTransform(int[] data, Random random) //Three numbers
        {
            int[] rowShuffle = [0, 1, 2];
            int[] columnShuffle = [0, 1, 2];
            int[] swapNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];

            random.Shuffle(rowShuffle);
            random.Shuffle(columnShuffle);
            random.Shuffle(swapNumbers);
            int[] flippedData = SwapRowsColumns(SwapNumbers(Mirror(Mirror(FlipData(data), true), true), swapNumbers), rowShuffle, columnShuffle);
            return flippedData;
        }
    }
}