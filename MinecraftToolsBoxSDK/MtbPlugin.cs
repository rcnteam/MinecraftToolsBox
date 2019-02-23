using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using MinecraftToolsBox;

namespace MinecraftToolsBoxSDK
{
    public abstract class MtbPlugin
    {
        public static MtbPlugin Plugin;
        public string Name;
        public string Description;
        public string Version;
        public string Author;
        public string UpdateMessage;
        public BitmapImage Icon;

        public MtbPlugin()
        {
            Plugin = this;
        }

        public abstract bool CheckUpdate();
        public abstract void LoadPlugin();
        public abstract void UnloadPlugin();
        public abstract Dictionary<string, Tile[]> GetPages();
    }
    public interface ICommandGenerator
    {
        string GenerateCommand();
    }
    public interface IMainWindowCommands
    {
        MtbWindow AddWindow(ImageSource Icon, object Content, string Title, MtbPlugin Plugin);
        void ShowWindow(MtbWindow Window);
        void CloseWindow(MtbWindow mtbWindow);

        void ShowWindowView(StackPanel Panel, int Offset);
        void CloseWindowView();
        DragControlHelper GetHelper();
    }
    public interface IMtbWindow
    {
        void ShowDialog(object content, Button Close, Button Cancel);
    }
}
