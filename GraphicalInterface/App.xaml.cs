using System.Windows;

namespace GraphicalInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void Start()
        {
            Thread wpfThread = new Thread(() =>
            {
                App app = new App();
                app.Run(new MainWindow());
            });
            wpfThread.SetApartmentState(ApartmentState.STA);
            wpfThread.Start();
        }
    }
}