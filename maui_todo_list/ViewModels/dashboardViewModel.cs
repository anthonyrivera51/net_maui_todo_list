using System;
using System.Collections.ObjectModel;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using maui_todo_list.Model;
using GraphQL;
using maui_todo_list.Services;

namespace maui_todo_list.ViewModels
{
    public class dashboardViewModel
    {
        private readonly GraphQLHttpClient _graphQLHttpClient;
        public ObservableCollection<City> items { get; set; }

        public dashboardViewModel()
        {
            _graphQLHttpClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("https://cjjfg4vl-3002.use.devtunnels.ms/graphql")
            }, new NewtonsoftJsonSerializer());

            string userDetailsStr = Preferences.Get(nameof(App.UserAgents), "");
            var listUsersAgents = JsonConvert.DeserializeObject<Users>(userDetailsStr);

            // Agrega headers personalizados
            _graphQLHttpClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", listUsersAgents.token);

        }

        public async void LoadData()
        {
            try
            {
                var cities = new List<City>();
                var request = new GraphQLRequest
                {
                    Query = @"query findAllVisitDashboard {
                      findAllVisitDashboard {
                        earrings {
                          dateVisit
                          client {
                            name
                          }
                        }
                        realized {
                          client {
                            id
                        }
                      }
                    }",
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

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}

