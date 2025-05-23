using DonutGame.views;

namespace DonutGame;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (s, e) => 
            MessageBox.Show($"Error: {e.Exception.Message}", "Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
    
        AppDomain.CurrentDomain.UnhandledException += (s, e) => 
            MessageBox.Show($"Fatal error: {(e.ExceptionObject as Exception)?.Message}", "Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainMenuForm());
    }
}