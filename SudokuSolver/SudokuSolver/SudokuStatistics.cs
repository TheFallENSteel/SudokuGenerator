using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuStatistics
    {
        public bool HasOneSolution => SolutionsCount == 1;
        public bool IsSolvable => SolutionsCount > 0;
        public int SolutionsCount;
        public int MinStepsInRound;
        public int MissingSteps;
        public double AverageMinSteps;
    }
}
