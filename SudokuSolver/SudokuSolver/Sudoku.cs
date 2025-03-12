using System;

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
        public Sudoku Solve(int difficulty)
        {
            Sudoku solution = new Sudoku(this.GetData());
            while (true)
            {
                if (solution.IsSolved()) break;
                else if (difficulty >= 1 && solution.SudokuData.CheckForHiddenValues()) continue;
                else if (difficulty >= 2 && solution.SudokuData.CheckForNakedValues()) continue;
                else if (difficulty >= 3 && solution.SudokuData.CheckForPointing()) continue;
                else if (difficulty >= 4 && solution.SudokuData.CheckForClaiming()) continue;
                else break;
            }
            return solution;
        }

        public void UnSolve(int difficulty)
        {
            Sudoku solution = new Sudoku(this.GetData());
            for (int i = 0; i < 81; i++)
            {
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

        /*public static (Sudoku, SudokuData?) Guess(SudokuData lastSolution, int difficulty)
        {
            Cell cell = lastSolution.LeastVariableUnsetCell();
            int[] possibilities = cell.Possibilities.ToArray();
            foreach (var possibility in possibilities)
            {
                Sudoku solution = new Sudoku(lastSolution.GetData());
                solution.SudokuData.GetCell(cell.x, cell.y).TrySetValue(possibility);
                var tempSolution = solution.Solve(difficulty, true);
                if (tempSolution != null) 
                { 
                    return (solution, tempSolution);
                }
            }
            return (null, null);
        }*/

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
