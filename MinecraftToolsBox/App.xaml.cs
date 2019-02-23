using System.Windows;

namespace MinecraftToolsBox
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ShowException window = new ShowException(e.Exception);
            window.Show();
        }
    }
}
