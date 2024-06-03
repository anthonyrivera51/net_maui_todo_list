
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using maui_todo_list.Model;
using maui_todo_list.Pages.Dashboard;
using Newtonsoft.Json;

namespace maui_todo_list.Pages.Login;

public partial class SignIn : ContentPage
{
    private readonly GraphQLHttpClient _graphQLHttpClient;

    public SignIn()
    {
        InitializeComponent();

        ValidateSession();

        _graphQLHttpClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
        {
            EndPoint = new Uri("https://cjjfg4vl-3002.use.devtunnels.ms/graphql")
        }, new NewtonsoftJsonSerializer());
    }

    private async void ValidateSession()
    {
        string userDetailsStr = Preferences.Get(nameof(App.UserAgents), "");
        if (string.IsNullOrWhiteSpace(userDetailsStr))
        {
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(Dashboard));
        }
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        if (txtEmail.Text?.Length == null || txtPassword.Text?.Length == null)
        {
            await DisplayAlert("Alert", "Email or Password cannot be empty", "Retry");
            return;
        }
        await LoadData(txtEmail.Text, txtPassword.Text);
        await Shell.Current.GoToAsync(nameof(Dashboard));
        //Application.Current.MainPage = new MainPage(); // = new Dashboard();

    }

    private async Task LoadData(string email, string password)
    {
        try
        {
            // Define la consulta GraphQL
            var request = new GraphQLRequest
            {
                Query = @"mutation Signin($signinInput: SigninInput!) { signin(signinInput: $signinInput) { token user { name identificationNumber id email } } }",
                Variables = new
                {
                    SigninInput = new
                    {
                        email = email, //"amolina@gmail.com",
                        password = password, // "123456789"
                    }
                }

            };

            // Ejecuta la consulta
            var response = await _graphQLHttpClient.SendMutationAsync<dynamic>(request);
            var datos = response.Data.signin;

            Console.WriteLine($"{datos.user}");

            Users users = new Users();
            users.token = datos.token;
            users.name = datos.user.name;
            users.identificationNumber = datos.user.identificationNumber;
            users.id = datos.user.id;
            users.email = datos.user.email;

            App.UserAgents = users;
            string userInJSON = JsonConvert.SerializeObject(users);
            Preferences.Set(nameof(App.UserAgents), userInJSON);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }


    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // await Shell.Current.GoToAsync(nameof(Register));
    }

    private async void OnForgetPasswordClicked(object sender, EventArgs e)
    {
        // await Shell.Current.GoToAsync(nameof(ForgetPassword));
    }
}
