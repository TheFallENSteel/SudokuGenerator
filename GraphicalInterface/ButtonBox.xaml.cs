using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace GraphicalInterface
{
    /// <summary>
    /// Interaction logic for ButtonBox.xaml
    /// </summary>
    public partial class ButtonBox : UserControl
    {
        public SudokuBrowser SudokuBrowser
        {
            get;
            set;
        }
        public ButtonBox(SudokuBrowser sudokuBrowser)
        {
            this.SudokuBrowser = sudokuBrowser;
            InitializeComponent();
        }
        private void prev_Click(object sender, RoutedEventArgs e)
        {
            SudokuBrowser.Previous();
        }
        private void next_Click(object sender, RoutedEventArgs e)
        {
            SudokuBrowser.Next();
        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
            SudokuBrowser.AddSudokuData(new SudokuData(), true);
        }
        private void remove_Click(object sender, RoutedEventArgs e)
        {
            SudokuBrowser.Remove();
        }
        private void generate_Click(object sender, RoutedEventArgs e)
        {
            SudokuBrowser.GetSolution(SudokuBrowser.Difficulty);
        }
        private void print_Click(object sender, RoutedEventArgs e)
        {
            SudokuBrowser.Print();
        }
        private void solve_Click(object sender, RoutedEventArgs e)
        {
            SudokuBrowser.Solve(SudokuBrowser.Difficulty);
        }
        private void randomize_Click(object sender, RoutedEventArgs e)
        {
            SudokuBrowser.RandomizeSudoku();
        }
        private void load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".sudoku",
                AddToRecent = true,
                Filter = "Sudoku Files|*.sudoku",
                Title = "Save Sudoku",
                ValidateNames = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
            };
            bool result = openFileDialog.ShowDialog() == true;
            if (result)
            {
                SudokuBrowser.LoadSudoku(openFileDialog.OpenFile());
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".sudoku",
                AddToRecent = true,
                OverwritePrompt = true,
                Filter = "Sudoku Files|*.sudoku",
                Title = "Save Sudoku",
                ValidateNames = true,
            };
            bool result = saveFileDialog.ShowDialog() == true;
            if (result)
            {
                SudokuBrowser.SaveSudoku(saveFileDialog.OpenFile());
            }
        }

        private void difficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SudokuBrowser.Difficulty = difficulty.SelectedIndex;
        }

    }
}
