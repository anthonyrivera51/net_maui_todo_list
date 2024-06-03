
using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using maui_todo_list.Model;
using maui_todo_list.Pages.Login;
using maui_todo_list.Pages.Settings;
using Newtonsoft.Json;

namespace maui_todo_list.Pages.Dashboard;

public partial class Dashboard : ContentPage
{
    private readonly GraphQLHttpClient _graphQLHttpClient;

    public Dashboard()
    {
        string userDetailsStr = Preferences.Get(nameof(App.UserAgents), "");
        var listUsersAgents = JsonConvert.DeserializeObject<Users>(userDetailsStr);

        _graphQLHttpClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
        {
            EndPoint = new Uri("https://cjjfg4vl-3002.use.devtunnels.ms/graphql")
        }, new NewtonsoftJsonSerializer());


        // Agrega headers personalizados
        _graphQLHttpClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", listUsersAgents.token);

        LoadData();

        InitializeComponent();

        if (listUsersAgents != null)
        {
            txtName.Text = listUsersAgents.name;
        }
    }

    async void LoadData()
    {
        try
        {
            var request = new GraphQLRequest
            {
                Query = @"query findAllVisitDashboard { findAllVisitDashboard { earrings { dateVisit client { name } } realized { client { id } } } }"
            };

            GraphQLResponse<dynamic> response = await _graphQLHttpClient.SendQueryAsync<dynamic>(request);

            string jsonString = JsonConvert.SerializeObject(response.Data, Formatting.Indented); // Specify formatting (optional);


        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }

    async void ImageButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (Preferences.ContainsKey(nameof(App.UserAgents)))
        {
            Preferences.Remove(nameof(App.UserAgents));
        }

        await Shell.Current.GoToAsync(nameof(SignIn));
    }

    async void ImageButton_Clicked_1(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushModalAsync(new SettingsPage());
    }

}
