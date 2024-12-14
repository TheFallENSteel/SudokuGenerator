using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Sudoku
    {
        public SudokuStatistics Statistics { get; private set; }
        private SudokuData SudokuData { get; set; }
        public static int Size => SudokuData.Size;
        public Sudoku(int[] data, bool toBeValidated = false)
        {
            SudokuData = new SudokuData(data, toBeValidated);
            Statistics = new SudokuStatistics();
        }

        //public Sudoku(Random random) : this(GenerateRandomSolution()) { }

        /*private static readonly int[] randomSudokuBase =
            [
                1, 2, 3,    4, 5, 6,    7, 8, 9,
                7, 8, 9,    1, 2, 3,    4, 5, 6,
                4, 5, 6,    7, 8, 9,    1, 2, 3,

                9, 1, 2,    3, 4, 5,    6, 7, 8,
                6, 7, 8,    9, 1, 2,    3, 4, 5,
                3, 4, 5,    6, 7, 8,    9, 1, 2,

                8, 9, 1,    2, 3, 4,    5, 6, 7,
                5, 6, 7,    8, 9, 1,    2, 3, 4,
                2, 3, 4,    5, 6, 7,    8, 9, 1
            ];*/
            //Array.Copy(randomSudokuBase, data, 81);

        /*private static int[] GenerateRandomSolution()
        {
            int[] data = new int[81];


            return sudokuData.GetData();
        }*/ //TODO

        public void SetSudoku(Sudoku sudoku) 
        { 
            this.SudokuData = sudoku.SudokuData;
            this.Statistics = sudoku.Statistics;
        }

        public int[] GetData() 
        { 
            return SudokuData.GetData();
        }

        static int totalMin = int.MaxValue;
        public SudokuData? Solve(int difficulty, bool canGuess = false)
        {
            Sudoku solution = new Sudoku(this.GetData());
            int numberOfSteps = -1;
            while (true)
            {
                //Debug.WriteLine(solution.ToFormattedString());
                numberOfSteps++;
                if (solution.IsSolved()) break;
                if (solution.SudokuData.CheckForHiddenValues()) 
                {
                    //if (difficulty > 1) this.SetSudoku(solution);
                    continue;
                }
                if (solution.SudokuData.CheckForNakedValues()) 
                {
                    //if (difficulty > 2) this.SetSudoku(solution);
                    continue; 
                }
                Sudoku? newBase; SudokuData? newSolution;
                if (canGuess && ((newBase, newSolution) = Guess(solution.SudokuData, difficulty)) != (null, null)) 
                {
                    solution.SudokuData = newSolution;
                    SetSudoku(newBase);
                }
                break;
            }
            Statistics.MissingSteps = numberOfSteps;
            return solution.IsSolved() ? solution.SudokuData : null;
        }

        public void UnSolve(int difficulty)
        {
            Sudoku solution = null;
            for (int i = 0; i < 81; i++)
            {
                solution = new Sudoku(this.GetData());
                int value = solution.SudokuData.Cells[i].Value;
                if (value != 0)
                {
                    solution.SudokuData.Cells[i].RemoveValue(value);
                }
                var solved = solution.Solve(difficulty);
                Debug.WriteLine(solution.ToFormattedString());
                if (solved == null || !solved.IsSolved()) solution.SudokuData.Cells[i].TrySetValue(value);
            }
            this.SetSudoku(solution);
        }

        public static (Sudoku, SudokuData?) Guess(SudokuData lastSolution, int difficulty)
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
