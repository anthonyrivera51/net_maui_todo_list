
using maui_todo_list.Pages.Dashboard;
using maui_todo_list.Pages.Login;
using maui_todo_list.Pages.Settings;

namespace maui_todo_list;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        RegisterRoutes();
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(SignIn), typeof(SignIn));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(Dashboard), typeof(Dashboard));
    }
}

