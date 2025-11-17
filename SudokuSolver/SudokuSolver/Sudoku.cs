using System;
using System.Linq;

namespace SudokuSolver
{
    public class Sudoku
    {
        private SudokuData SudokuData { get; set; }
        public static int Size => SudokuData.Size;
        public Sudoku(int[] data, bool toBeValidated = false)
        {
            SudokuData = new SudokuData(data, toBeValidated);
        }

        public void SetSudoku(Sudoku sudoku)
        {
            this.SudokuData = sudoku.SudokuData;
        }

        public int[] GetData()
        {
            return SudokuData.GetRawData();
        }

        static int totalMin = int.MaxValue;
        public const int MAX_DIFFICULTY = 4;
        public Sudoku Solve(int difficulty, bool returnUnmodifiedIfNotSolved = false)
        {
            Sudoku solution = new Sudoku(this.GetData());
            if (this.SudokuData.GetEmptyCellsCount() > Size * Size - 17) return solution;
            while (true)
            {
                if (solution.IsSolved()) break;
                else if (difficulty >= 1 && solution.SudokuData.CheckForHiddenValues()) continue;
                else if (difficulty >= 2 && solution.SudokuData.CheckForNakedValues()) continue;
                else if (difficulty >= 3 && solution.SudokuData.CheckForPointing()) continue;
                else if (difficulty >= 4 && solution.SudokuData.CheckForClaiming()) continue;
                else if (returnUnmodifiedIfNotSolved) return new Sudoku(this.GetData());
                else break;
            }
            return solution;
        }

        public void UnSolve(int difficulty)
        {
            Sudoku solution = new Sudoku(this.GetData());
            int[] indexes = Enumerable.Range(0, 81).ToArray();
            Random random = new Random();
            random.Shuffle(indexes);
            for (int j = 0; j < 81; j++)
            {
                int i = indexes[j];
                int value = solution.SudokuData.Cells[i].Value;
                if (value != 0)
                {
                    solution.SudokuData.Cells[i].RemoveValue();
                }
                Sudoku? solved = solution.Solve(difficulty);
                if (!solved.IsSolved()) solution.SudokuData.Cells[i].TrySetValue(value);
            }
            this.SetSudoku(solution);
        }

        public static Sudoku GenerateRandomSolved()
        {
            int[] tempGrid = new int[81];
            for (int i = 0; i < tempGrid.Length; i++)
            {
                tempGrid[i] = 0;
            }
            Sudoku? tempSudoku = new Sudoku(tempGrid);
            tempSudoku = GenerateRandomSudoku(new Random(), tempSudoku);
            if (tempSudoku == null) throw new Exception("Failed to generate sudoku. Time exceeded");
            return tempSudoku;
        }
        public void RandomizeSudoku(Random random)
        {
            SudokuData.RandomTransform(SudokuData.GetRawData(), random);
        }

        private static Sudoku? GenerateRandomSudoku(Random random, Sudoku sudoku)
        {
            sudoku = sudoku.Solve(2);
            if (sudoku.IsSolved()) return sudoku;

            Cell? decisiveCell = sudoku.SudokuData.LeastVariableUnsetCell();
            if (decisiveCell == null) return null;
            int[] decisiveCellValues = new int[decisiveCell.Possibilities.Count];
            decisiveCell.Possibilities.ToArray().CopyTo(decisiveCellValues, 0);
            random.Shuffle(decisiveCellValues);

            int[] sudokuData = sudoku.GetData();

            for (int i = 0; i < decisiveCellValues.Length; i++)
            {
                sudokuData[decisiveCell.Index] = decisiveCellValues[i];
                Sudoku? tempSudoku = new Sudoku(sudokuData);
                tempSudoku = GenerateRandomSudoku(random, tempSudoku);
                if (tempSudoku != null && tempSudoku.IsSolved())
                {
                    return tempSudoku;
                }
            }
            return null;
        }

        public bool IsSolved() => SudokuData.IsSolved();

        public string ToFormattedString()
        {
            return SudokuData.ToFormattedString();
        }
        public override string ToString()
        {
            return SudokuData.ToString();
        }
    }
}
