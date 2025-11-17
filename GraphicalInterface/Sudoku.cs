using System.ComponentModel;
using System.Text;

namespace GraphicalInterface
{
    public class SudokuData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public const int Size = 9;
        public const int TotalSize = Size * Size;
        private int[] data = new int[TotalSize];
        public int this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                data[index] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(data)));
            }
        }
        public int this[int x, int y]
        {
            get
            {
                return this[y * Size + x];
            }
            set
            {
                this[y * Size + x] = value;
            }
        }
        public int[] GetData()
        {
            return data;
        }
        public SudokuData()
        {
            this.data = new int[TotalSize];
        }
        public SudokuData(string data)
        {
            this.LoadFromString(data);
        }
        public SudokuData(int[] data)
        {
            this.data = data;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int j = 0; j < Size; j++)
            {
                for (int i = 0; i < Size; i++)
                {
                    stringBuilder.Append(this[i, j].ToString()[0]);
                }
            }
            return stringBuilder.ToString();
        }
        private static int[] loadFromString(string source)
        {
            return source
                .ToCharArray()
                .Where(c => c != ' ')
                .Select(val =>
                {
                    int value = int.Parse(val.ToString());
                    return (value <= 9 && value >= 1) ? value : 0;
                })
                .ToArray();
        }
        public void LoadFromString(string source)
        {
            data = loadFromString(source);
        }
    }
}