
using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using maui_todo_list.Model;
using maui_todo_list.Services;
using Newtonsoft.Json;
using static Android.Content.ClipData;

namespace maui_todo_list.Pages.Settings;

public partial class SettingsPage : ContentPage
{
    private readonly GraphQLHttpClient _graphQLHttpClient;
    private String ClientId = "";
    private String VisitedId = "";
    private bool isToogle;
    public List<City> CitiesList;
    public List<VisitedTypes> VisitedTypes;
    public SettingsPage()
    {
        _graphQLHttpClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
        {
            EndPoint = new Uri("https://cjjfg4vl-3002.use.devtunnels.ms/graphql")
        }, new NewtonsoftJsonSerializer());

        string userDetailsStr = Preferences.Get(nameof(App.UserAgents), "");
        var listUsersAgents = JsonConvert.DeserializeObject<Users>(userDetailsStr);

        // Agrega headers personalizados
        _graphQLHttpClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", listUsersAgents.token);

        CitiesList = new List<City>();
        GetClients();
        GetVisitedTypes();

        InitializeComponent();

        ClientId = "";
        VisitedId = "";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing(); // do the usual stuff OnAppearing does
        NavigationPage.SetHasNavigationBar(this, true); //get your navbar back
        NavigationPage.SetHasBackButton(this, true); //get your back button back
    }

    public async void GetClients()
    {
        try
        {
            var cities = new List<City>();
            var request = new GraphQLRequest
            {
                Query = @"query Clients($where: FindClientWhere, $pagination: Pagination) {
                  clients(where: $where, pagination: $pagination) {
                    id
                    createdAt
                    updatedAt
                    deletedAt
                    name
                    numberDocument
                    email
                    telefono
                    celular
                  }
                }",
                Variables = new
                {
                    pagination = new
                    {
                        skip = 0,
                        take = 0
                    }
                }
            };


            GraphQLResponse<dynamic> response = await _graphQLHttpClient.SendQueryAsync<dynamic>(request);

            string jsonString = JsonConvert.SerializeObject(response.Data, Formatting.Indented); // Specify formatting (optional);

            ListClient listClient = JsonConvert.DeserializeObject<ListClient>(jsonString);

            for (int i = 0; i < listClient.clients.Count; i++)
            {
                City _clients = new City();
                _clients.Key = listClient.clients[i].id;
                _clients.Value = listClient.clients[i].name;

                cities.Add(_clients);
            }

            pickerItem.ItemsSource = cities;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }

    public async void GetVisitedTypes()
    {
        try
        {
            var visited = new List<VisitedTypes>();
            var request = new GraphQLRequest
            {
                Query = @"query VisitTypes($orderBy: [FindVisitTypeOrderBy!]) {
                    visitTypes(orderBy: $orderBy) {
                    id
                    createdAt
                    updatedAt
                    deletedAt
                    name
                    description
                    status
                    }
                }",
                Variables = new
                {
                    orderBy = new
                    {
                        createdAt = "ASC",
                    }
                }
            };


            GraphQLResponse<dynamic> response = await _graphQLHttpClient.SendQueryAsync<dynamic>(request);

            string jsonString = JsonConvert.SerializeObject(response.Data, Formatting.Indented); // Specify formatting (optional);

            ListVisited listClient = JsonConvert.DeserializeObject<ListVisited>(jsonString);

            for (int i = 0; i < listClient.visitTypes.Count; i++)
            {
                VisitedTypes _clients = new VisitedTypes();
                _clients.Key = listClient.visitTypes[i].id;
                _clients.Value = listClient.visitTypes[i].name;

                visited.Add(_clients);
            }

            pickerVisit.ItemsSource = visited;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }

    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        if (string.IsNullOrEmpty(ClientId) || string.IsNullOrWhiteSpace(ClientId))
        {
            await DisplayAlert("Error", "Seleccione un Cliente.", "Intente");
            return;
        }

        if (string.IsNullOrEmpty(VisitedId) || string.IsNullOrWhiteSpace(VisitedId))
        {
            await DisplayAlert("Error", "Seleccione un Tipo de Visita.", "Intente");
            return;
        }

        string userDetailsStr = Preferences.Get(nameof(App.UserAgents), "");
        var listUsersAgents = JsonConvert.DeserializeObject<Users>(userDetailsStr);

        var request = new GraphQLRequest
        {
            Query = @"mutation CreateVisit($createInput: CreateVisitInput!) {
                  createVisit(createInput: $createInput) {
                    id
                    client {
                      name
                    }
                  }
                }",
            Variables = new
            {
                createInput = new
                {
                    clientId = ClientId,
                    dateVisit = MyDatePicker.Date.ToString("yyyy-MM-dd"),
                    description = txtDescription.Text,
                    isProyect = isToogle,
                    status = "programmed",
                    userId = listUsersAgents.id,
                    typeId = VisitedId
                }
            }
        };

        var response = await _graphQLHttpClient.SendMutationAsync<dynamic>(request);

        if (response.Data == null)
        {
            await DisplayAlert("Error", response.Errors.ToString(), "Intente nuevamente");
            return;
        }

        await DisplayAlert("Ok", "Visita Creada.", "Ok");
        await Navigation.PopAsync();
    }

    void pickerItem_SelectedIndexChanged(System.Object sender, System.EventArgs e)
    {
        var selectedItem = (City)pickerItem.SelectedItem;
        ClientId = selectedItem.Key;
    }

    void pickerVisit_SelectedIndexChanged(System.Object sender, System.EventArgs e)
    {
        var selectedItem = (VisitedTypes)pickerVisit.SelectedItem;
        VisitedId = selectedItem.Key;
    }

    void switchedIsProyect_Toggled(System.Object sender, Microsoft.Maui.Controls.ToggledEventArgs e)
    {
        isToogle = switchedIsProyect.IsToggled;
    }

    async void Button_Clicked_1(System.Object sender, System.EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
