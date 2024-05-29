
using maui_todo_list.Pages.Dashboard;
using maui_todo_list.Pages.Settings;
using Microsoft.Extensions.Logging;

namespace maui_todo_list;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<Dashboard>();
        builder.Services.AddSingleton<SettingsPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

