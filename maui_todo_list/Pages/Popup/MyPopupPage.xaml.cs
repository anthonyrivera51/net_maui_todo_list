namespace maui_todo_list.Pages.Popup;

public partial class MyPopupPage
{
    private string uidVisited;
    public MyPopupPage(string uid)
    {
        this.uidVisited = uid;
        InitializeComponent();
    }
}
