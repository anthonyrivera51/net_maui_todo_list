
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using maui_todo_list.Model;
using maui_todo_list.Pages.Login;
using maui_todo_list.Pages.Popup;
using maui_todo_list.Pages.Settings;
using Mopups.Interfaces;
using Newtonsoft.Json;
using static maui_todo_list.Model.VisitedList;

namespace maui_todo_list.Pages.Dashboard;

public partial class Dashboard : ContentPage
{
    private readonly GraphQLHttpClient _graphQLHttpClient;
    List<VisitedList.Earring> listEarning = null;

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
            listEarning = new List<VisitedList.Earring>();

            var request = new GraphQLRequest
            {
                Query = @"query findAllVisitDashboard { findAllVisitDashboard { earrings { id dateVisit client { name } } realized { client { id } } } }"
            };

            GraphQLResponse<dynamic> response = await _graphQLHttpClient.SendQueryAsync<dynamic>(request);

            if (response.Errors != null)
            {
                await Shell.Current.Navigation.PopToRootAsync();

                await Shell.Current.GoToAsync($"//{nameof(SignIn)}");
                return;
            }

            string jsonString = JsonConvert.SerializeObject(response.Data, Formatting.Indented); // Specify formatting (optional);
            VisitedList list = JsonConvert.DeserializeObject<VisitedList>(jsonString);

            for (int i = 0; i < list.findAllVisitDashboard.earrings.Count; i++)
            {
                VisitedList.Earring earring = new VisitedList.Earring();
                earring.id = list.findAllVisitDashboard.earrings[i].id;
                earring.client = new VisitedList.Client
                {
                    name = list.findAllVisitDashboard.earrings[i].client.name
                };
                earring.dateVisit = list.findAllVisitDashboard.earrings[i].dateVisit;

                listEarning.Add(earring);
            }

            listVisitedPending.ItemsSource = listEarning;
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

    async void btnListButton_Clicked(System.Object sender, System.EventArgs e)
    {
        var item = sender as Button;
        if (item == null) return;

        var m = item.BindingContext as Earring;
        if (m == null) return;

        bool answer = await DisplayAlert("Visita", "¿Realizo la visita o desea cancelarla?", "Realizar Visita", "Cancelar Visita");
        if (answer)
        {

            var request = new GraphQLRequest
            {
                Query = @"mutation UpdateVisit($updateInput: UpdateVisitInput!) {
                  updateVisit(updateInput: $updateInput) {
                    id
                  }
                }",
                Variables = new
                {
                    updateInput = new
                    {
                        id = m.id,
                        status = "realized"
                    }
                }
            };
            string userDetailsStr = Preferences.Get(nameof(App.UserAgents), "");
            var listUsersAgents = JsonConvert.DeserializeObject<Users>(userDetailsStr);

            // Agrega headers personalizados
            _graphQLHttpClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", listUsersAgents.token);

            var response = await _graphQLHttpClient.SendMutationAsync<dynamic>(request);

            if (response.Data == null)
            {
                await DisplayAlert("Error", response.Errors.ToString(), "Intente nuevamente");
                return;
            }

        }
        else
        {
            var request = new GraphQLRequest
            {
                Query = @"mutation UpdateVisit($updateInput: UpdateVisitInput!) {
                  updateVisit(updateInput: $updateInput) {
                    id
                  }
                }",
                Variables = new
                {
                    updateInput = new
                    {
                        id = m.id,
                        status = "canceled"
                    }
                }
            };

            string userDetailsStr = Preferences.Get(nameof(App.UserAgents), "");
            var listUsersAgents = JsonConvert.DeserializeObject<Users>(userDetailsStr);
            // Agrega headers personalizados
            _graphQLHttpClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", listUsersAgents.token);
            var response = await _graphQLHttpClient.SendMutationAsync<dynamic>(request);

            if (response.Data == null)
            {
                await DisplayAlert("Error", response.Errors.ToString(), "Intente nuevamente");
                return;
            }
        }

        LoadData();
    }

}
