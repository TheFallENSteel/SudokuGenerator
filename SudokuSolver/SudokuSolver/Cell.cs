using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Cell
    {
        public readonly int x;
        public readonly int y;
        public int Index => x + y * 9;
        public List<int> Possibilities { get; private set; } = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        private List<Container> Containers { get; } = [];
        public bool IsDefinitive => Possibilities.Count == 0;
        private int cellValue;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void RemoveValue()
        {
            Possibilities = [1, 2, 3, 4, 5, 6, 7, 8, 9];
            for (int i = 0; i < Containers.Count; i++)
            {
                Containers[i].Possibilities.Add(cellValue);
            }
            cellValue = 0;
        }

        public int Value 
        { 
            get 
            {
                return cellValue;
            }
            private set 
            { 
                cellValue = value;
            }
        }
        public bool TrySetValue(int value) 
        {
            if (value != 0 && cellValue == 0 && Possibilities.Contains(value))
            {
                RemovePossibilities(value);
                return true;
            }
            return value == 0;
        }

        private void RemovePossibilities(int value)
        {
            Value = value;
            Possibilities.Clear();
            for (int i = 0; i < Containers.Count; i++)
            {
                Containers[i].NumberSet(value);
            }
        }

        public bool ConfirmDefinitiveValue()
        {
            if (Possibilities.Count == 1)
            {
                Value = Possibilities[0];
                Possibilities.Clear();
                for (int i = 0; i < Containers.Count; i++)
                {
                    Containers[i].NumberSet(Value);
                }
                return true;
            }
            return false;
        }
        public bool RemovePossibility(int value) 
        { 
            return Possibilities.Remove(value);
        }
        public bool IsPossible(int value) 
        {
            return Possibilities.Contains(value);
        }
        public void AddContainer(Container container) 
        {
            if (!Containers.Contains(container))
            {
                Containers.Add(container);
            }
        }
        public override string ToString()
        {
            return $"Cell[{x}, {y}]";
        }
    }
}
