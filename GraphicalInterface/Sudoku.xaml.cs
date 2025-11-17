using Microsoft.Extensions.Primitives;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphicalInterface
{
    /// <summary>
    /// Interaction logic for Sudoku.xaml
    /// </summary>
    public partial class Sudoku : UserControl
    {
        private int selectedIndex = -1;
        private readonly static Brush cellBackground = Brushes.White;
        private readonly static Brush sudokuBackground = Brushes.Gray;
        private readonly static double squareSeparatorSize = 1;
        private readonly static double cellSeparatorSize = 0.1;
        public SudokuData SudokuData
        {
            get
            {
                return sudokuData;
            }
            set
            {
                sudokuData = value;
                SudokuData.PropertyChanged += OnSudokuUpdate;
                OnSudokuUpdate(this, new PropertyChangedEventArgs(""));
            }
        }
        public StringBuilder StringValue = new StringBuilder(SudokuData.TotalSize + 2);
        public SudokuData sudokuData = new SudokuData();
        private TextBox[] labels = new TextBox[SudokuData.TotalSize];
        public Sudoku()
        {
            InitializeComponent();
            this.Background = sudokuBackground;
            this.KeyDown += this.Sudoku_KeyDown;
            sudokuGrid.Rows = SudokuData.Size;
            sudokuGrid.Columns = SudokuData.Size;
            for (int y = 0; y < SudokuData.Size; y++)
            {
                for (int x = 0; x < SudokuData.Size; x++)
                {
                    int index = y * SudokuData.Size + x;
                    StringValue.Append(' ');
                    double rightThickness = cellSeparatorSize + (x % 3 == 2 ? 1 : 0) * squareSeparatorSize;
                    double leftThickness = cellSeparatorSize + (x % 3 == 0 ? 1 : 0) * squareSeparatorSize;
                    double bottomThickness = cellSeparatorSize + (y % 3 == 2 ? 1 : 0) * squareSeparatorSize;
                    double topThickness = cellSeparatorSize + (y % 3 == 0 ? 1 : 0) * squareSeparatorSize;
                    Viewbox viewBox = new Viewbox()
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Stretch = Stretch.Uniform,
                    };
                    labels[index] = new TextBox()
                    {
                        Margin = new Thickness(leftThickness, topThickness, rightThickness, bottomThickness),
                        Width = 30,
                        Height = 30,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Background = Sudoku.cellBackground,
                        AutoWordSelection = true,
                        Name = $"label{index}",

                    };
                    viewBox.Child = labels[index];
                    labels[index].TextChanged += ((o, e) =>
                    {
                        //if (e.Changes.Count == 1) return;
                        if (!int.TryParse(labels[index].Text, out int value)) value = 0;
                        if (value < 0 || value > 9) value = 0;
                        SudokuData[int.Parse(((TextBox)o).Name.Substring(5))] = value;
                    });
                    labels[index].PreviewKeyDown += ((o, e) =>
                    {
                        if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
                        {
                            Sudoku_KeyDown(o, e);
                        }
                    });
                    labels[index].GotFocus += ((o, e) =>
                    {
                        int index = int.Parse(((TextBox)o).Name.Substring(5));
                        selectedIndex = index;
                        labels[index].Dispatcher.BeginInvoke(new Action(() =>
                        {
                            labels[index].SelectAll();
                        }));
                    });
                    sudokuGrid.Children.Add(viewBox);
                }
            }
        }

        private void Sudoku_KeyDown(object sender, KeyEventArgs e)
        {
            //Allow moving with arrows
            switch (e.Key)
            {
                case Key.Left:
                    selectedIndex = selectedIndex - selectedIndex % SudokuData.Size + (selectedIndex + SudokuData.Size - 1) % SudokuData.Size;
                    break;
                case Key.Right:
                    selectedIndex = selectedIndex - selectedIndex % SudokuData.Size + (selectedIndex + SudokuData.Size + 1) % SudokuData.Size;
                    break;
                case Key.Up:
                    selectedIndex = (selectedIndex - SudokuData.Size + SudokuData.TotalSize) % SudokuData.TotalSize;
                    break;
                case Key.Down:
                    selectedIndex = (selectedIndex + SudokuData.Size + SudokuData.TotalSize) % SudokuData.TotalSize;
                    break;
            }
            if (selectedIndex > -1)
            {
                Keyboard.Focus(labels[selectedIndex]);
                labels[selectedIndex].Focus();
                labels[selectedIndex].SelectAll();
            }
        }

        public void OnSudokuUpdate(object? o, PropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                
                for (int i = 0; i < SudokuData.TotalSize; i++)
                {
                    labels[i].Text = (SudokuData[i] != 0) ? SudokuData[i].ToString() : "";
                    StringValue[i] = SudokuData[i].ToString()[0];
                }
                sudokuStringValue.Text = StringValue.ToString();
            }), System.Windows.Threading.DispatcherPriority.DataBind);
        }

        private void sudokuStringValue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(sudokuStringValue.Text);
        }
    }
}
