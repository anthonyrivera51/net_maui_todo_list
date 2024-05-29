
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using maui_todo_list.Pages.Dashboard;

namespace maui_todo_list.Pages.Login;

public partial class SignIn : ContentPage
{
    private readonly GraphQLHttpClient _graphQLHttpClient;

    public SignIn()
    {
        InitializeComponent();

        _graphQLHttpClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
        {
            EndPoint = new Uri("https://cjjfg4vl-3002.use.devtunnels.ms/graphql")
        }, new NewtonsoftJsonSerializer());
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        if (txtEmail.Text?.Length == 0 || txtPassword.Text?.Length == 0)
        {
            await DisplayAlert("Alert", "Email or Password cannot be empty", "Retry");
            return;
        }
        await LoadData();
        await Shell.Current.GoToAsync(nameof(Dashboard));
        //Application.Current.MainPage = new MainPage(); // = new Dashboard();

    }

    private async Task LoadData()
    {
        // Define la consulta GraphQL
        var request = new GraphQLRequest
        {
            Query = @"mutation Signin($signinInput: SigninInput!) { signin(signinInput: $signinInput) { token user { name identificationNumber id email }}}",
            Variables = new
            {
                signinInput = new
                {
                    email = "amolina@gmail.com",
                    password = "123456789"
                }
            }

        };

        // Ejecuta la consulta
        var response = await _graphQLHttpClient.SendMutationAsync<dynamic>(request);
        var datos = response.Data.signin;

        // Actualiza la UI con los datos
        BindingContext = datos;
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
