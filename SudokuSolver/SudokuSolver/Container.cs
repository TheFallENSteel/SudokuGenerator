using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class Container
    {
        private readonly int id;
        private readonly string type;
        public List<Cell> Cells;

        public Container(Cell[] cells, int id, string type)
        {
            this.id = id;
            this.type = type;
            this.Cells = cells.ToList();
            Cells.ForEach(cell => cell.AddContainer(this));
        }

        public List<int> Possibilities = [1, 2, 3, 4, 5, 6, 7, 8, 9];

        public int Sum() => Cells.Sum(cell => cell.Value);
        public bool IsSolved() => Sum() == 45;

        public List<Cell> Intersection(Container second) => this.Cells.Intersect(second.Cells).ToList();

        public List<Cell> Exclusion(Container second) => this.Cells.Except(second.Cells).ToList();

        public bool CheckForHiddenValues()
        {
            for (int i = 0; i < Possibilities.Count; i++)
            {
                int possibility = Possibilities[i];
                Cell? lastCell = null;
                int counter = 0;
                for (int j = 0; j < Cells.Count; j++)
                {
                    if (Cells[j].IsPossible(possibility))
                    {
                        lastCell = Cells[j];
                        counter++;
                    }
                }
                if (counter == 1 && lastCell.TrySetValue(possibility))
                {
                    return true;
                }
            }
            return false;
        }
        public void NumberSet(int value)
        {
            Possibilities.Remove(value);
            Cells.ForEach(tempCell =>
            {
                if (tempCell.Value == 0)
                {
                    tempCell.RemovePossibility(value);
                }
            });
        }

        public void AddCell(Cell cell)
        {
            if (!Cells.Contains(cell))
            {
                Cells.Add(cell);
            }
            cell.AddContainer(this);
        }
        public override string ToString()
        {
            return $"{type}({id})";
        }
    }
}
