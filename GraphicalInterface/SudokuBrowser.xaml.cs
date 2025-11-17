using PDFGenerator;
using SudokuSolver;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GraphicalInterface
{
    /// <summary>
    /// Interaction logic for SudokuBrowser.xaml
    /// </summary>
    public partial class SudokuBrowser : UserControl
    {
        private int CurrentIndex
        {
            get;
            set
            {
                field = value;
                UpdateSudokuData();
            }
        }
        public int Difficulty { get; set; } = 2;
        private List<SudokuData> SudokuList = new List<SudokuData>();
        public void AddSudokuData(SudokuData sudokuData, bool ChangeTo = false)
        {
            SudokuList.Add(sudokuData);
            if (ChangeTo) CurrentIndex = SudokuList.Count - 1;
        }
        public SudokuData CurrentSudokuData
        {
            get
            {
                if (SudokuList.Count <= 0)
                {
                    SudokuList.Add(new SudokuData());
                }
                return SudokuList[CurrentIndex];
            }
            set
            {
                if (SudokuList.Count <= 0)
                {
                    SudokuList.Add(new SudokuData());
                }
                SudokuList[CurrentIndex] = value;
                UpdateSudokuData();
            }
        }
        public int Next() => CurrentIndex = (SudokuList.Count == 0) ? CurrentIndex : (CurrentIndex + 1) % SudokuList.Count;
        public int Previous() => CurrentIndex = (SudokuList.Count == 0) ? CurrentIndex : (SudokuList.Count + CurrentIndex - 1) % SudokuList.Count;
        public void Add() => AddSudokuData(new SudokuData(), true);
        public void Remove()
        {
            if (SudokuList.Count > 0)
            {
                SudokuList.RemoveAt(CurrentIndex);
                CurrentIndex = Math.Max(0, CurrentIndex - 1);
            }
        }
        public void Solve(int difficulty)
        {
            try
            {
                SudokuSolver.Sudoku solution = new SudokuSolver.Sudoku(CurrentSudokuData.GetData()).Solve(difficulty, true);
                if (CurrentSudokuData.GetData() == solution.GetData() && !solution.IsSolved())
                {
                    MessageBox.Show($"Unsolvable sudoku with current difficulty", "Unsolvable", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                CurrentSudokuData = new SudokuData(solution.GetData());
            }
            catch (InvalidDataException)
            {
                MessageBox.Show($"Invalid Data in Sudoku", "Wrong data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private static int stackCapacity = 100;
        Stack<int[]> SudokuBuffer = new Stack<int[]>(3 * stackCapacity);
        int[] lastItem =
            [
            2, 7, 9, 8, 3, 4, 1, 6, 5,
            1, 8, 5, 6, 9, 7, 3, 4, 2,
            4, 3, 6, 2, 5, 1, 8, 9, 7,
            5, 4, 3, 9, 7, 8, 6, 2, 1,
            6, 1, 7, 4, 2, 3, 5, 8, 9,
            8, 9, 2, 1, 6, 5, 4, 7, 3,
            9, 6, 8, 3, 1, 2, 7, 5, 4,
            7, 2, 1, 5, 4, 6, 9, 3, 8,
            3, 5, 4, 7, 8, 9, 2, 1, 6,];
        bool isWorking = false;
        Thread? thread;
        public void GetSolution(int difficulty)
        {
            if (isWorking || (thread != null && thread.IsAlive)) return;
            thread = GetThread();
            thread.Start(difficulty);
        }
        Thread? generateSolutionsThread;
        public void GenerateSolutions()
        {
            if (generateSolutionsThread != null && generateSolutionsThread.IsAlive) return;
            generateSolutionsThread = new Thread(() =>
            {
                int timeSet = 1;
                while (true)
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(((double)SudokuBuffer.Count / stackCapacity + timeSet) * 500);
                    if (SudokuBuffer.Count < stackCapacity)
                    {
                        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                        CancellationToken cancellationToken = cancellationTokenSource.Token;
                        Task t = new Task(() =>
                        {
                            SudokuSolver.Sudoku solution = SudokuSolver.Sudoku.GenerateRandomSolved();
                            SudokuBuffer.Push(solution.GetData());
                        }, cancellationToken);
                        t.Start();
                        if (!t.Wait(timeout))
                        {
                            cancellationTokenSource.Cancel();
                            timeSet = int.Max(1, timeSet + 1);
                        }
                        else
                        {
                            timeSet = int.Max(1, timeSet - 1);
                        }
                    }
                }
            });
            generateSolutionsThread.Start();
        }
        public void Print()
        {
            SudokuData[] data = SudokuList.ToArray();
            PDFGenerator.PDFGenerator generator = new PDFGenerator.PDFGenerator(Environment.CurrentDirectory);
            generator.SaveSudokuDocument(
                new DocumentSettings(3, 2),
                data.Select(sud => new SudokuWrapper(sud)).ToArray(),
                "output.pdf");
            new Process
            {
                StartInfo = new ProcessStartInfo("output.pdf")
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                }
            }.Start();
        }
        public void RandomizeSudoku()
        {
            int[] data = SudokuSolver.SudokuData.RandomTransform(CurrentSudokuData.GetData(), random);
            CurrentSudokuData = new SudokuData(data);
        }
        private void UpdateSudokuData()
        {
            SudokuView.SudokuData = CurrentSudokuData;
        }
        public SudokuBrowser()
        {
            InitializeComponent();
            ButtonBox buttonBox = new ButtonBox(this);
            Grid.SetRow(buttonBox, 0);
            MainGrid.Children.Add(buttonBox);
            Next();
        }
        Random random = new Random();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateSolutions();
            random = new Random();
            thread = GetThread();
        }

        private Thread GetThread()
        {
            return new Thread((dif) =>
            {
                GenerateSolutions();
                int[] data;
                if (SudokuBuffer.Count != 0)
                {
                    data = SudokuBuffer.Pop();
                    lastItem = data;
                }
                else
                {
                    data = lastItem;
                }
                SudokuSolver.Sudoku solution = new SudokuSolver.Sudoku(data);
                solution.UnSolve((dif != null) ? (int)dif : int.MaxValue);
                SudokuData Result = new SudokuData(SudokuSolver.SudokuData.RandomTransform(solution.GetData(), random));
                CurrentSudokuData = Result;
            });
        }

        public void LoadSudoku(Stream stream)
        {
            SudokuList.Clear();
            StreamReader streamReader = new StreamReader(stream);
            string? line = null;
            while ((line = streamReader.ReadLine()) != null) 
            {
                this.AddSudokuData(new SudokuData(line));
            }
            stream.Close();
            UpdateSudokuData();
        }

        public void SaveSudoku(Stream stream)
        {
            StreamWriter streamReader = new StreamWriter(stream);
            SudokuList.ForEach(sudoku => stream.Write(Encoding.ASCII.GetBytes(sudoku.ToString() + Environment.NewLine)));
            stream.Close();
        }
    }
}
