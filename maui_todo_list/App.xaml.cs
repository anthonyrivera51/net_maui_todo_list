
using maui_todo_list.Model;

namespace maui_todo_list;

public partial class App : Application
{
    public static Users UserAgents;

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}

