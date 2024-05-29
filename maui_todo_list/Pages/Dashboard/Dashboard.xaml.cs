using maui_todo_list.Pages.Settings;

namespace maui_todo_list.Pages.Dashboard;

public partial class Dashboard : ContentPage
{
    public Dashboard()
    {
        InitializeComponent();
    }


    private async void OnCounterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }

}
