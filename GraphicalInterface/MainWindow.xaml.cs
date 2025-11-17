using System.Windows;

namespace GraphicalInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SudokuBrowser sudokuBrowser;
        public MainWindow()
        {
            sudokuBrowser = new SudokuBrowser();
            InitializeComponent();
            Main.Children.Add(sudokuBrowser);
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}