using System.Text;

namespace TodoListApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try
            {
                System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting console encoding: {ex.Message}");
            }

            MainPage = new AppShell();
        }
    }
}
