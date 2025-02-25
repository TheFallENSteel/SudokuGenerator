using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                throw new Exception("Invalid Sudoku data");
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
                if(Cells[i].ConfirmDefinitiveValue()) 
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

        public bool CheckForPointing() 
        {
            foreach (var square in SquareContainers)
            {
                foreach (var rowColumn in ColumnContainers.Concat(RowContainers))
                {
                    List<int> intersectionPossibilities = Cell.CellsPossibilities(square.Intersection(rowColumn));
                    List<int> exclusionPossibilities = Cell.CellsPossibilities(square.Exclusion(rowColumn));
                    List<int> pointingPossibilities = intersectionPossibilities.Except(exclusionPossibilities).ToList();
                    foreach (var possibility in pointingPossibilities)
                    {
                        //TODO: Implement pointing
                    }
                }
            }
            return false;
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
                RowContainers   [i] = new Container(GetRowCells(i), i, "Row");
                ColumnContainers[i] = new Container(GetColumnCells(i), i, "Column");
                SquareContainers[i] = new Container(GetSquareCells(i % SquareSize, i / SquareSize), i, "Square");
            }
        }
        public Container GetRow(int row) => RowContainers[row];
        public Container GetColumn(int column) => RowContainers[column];
        public Container GetSquare(int squareX, int squareY) => RowContainers[squareX + squareY * SquareSize];
        public Cell GetCell(int cellX, int cellY) => Cells[cellX + cellY * Size];
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
    }
}