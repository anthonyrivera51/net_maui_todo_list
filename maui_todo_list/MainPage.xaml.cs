
using maui_todo_list.Pages.Settings;

namespace maui_todo_list;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(SettingsPage)}");
    }

    private async void NavigationClientPage(object sender, EventArgs e)
    {
    }
}


