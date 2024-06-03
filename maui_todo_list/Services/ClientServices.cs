using System;
using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Java.Util;
using maui_todo_list.Model;
using Newtonsoft.Json;

namespace maui_todo_list.Services
{


    public class City
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class VisitedTypes
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    internal class ClientServices : IClient
    {
        private readonly GraphQLHttpClient _graphQLHttpClient;

        public ClientServices()
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

        private TaskCompletionSource<string> _dataTaskCompletionSource = new TaskCompletionSource<string>();

        public List<City> GetClient()
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

            var response = _graphQLHttpClient.SendQueryAsync<dynamic>(request).ConfigureAwait(false).GetAwaiter;

            string jsonString = JsonConvert.SerializeObject(response, Formatting.Indented); // Specify formatting (optional);

            ListClient listClient = JsonConvert.DeserializeObject<ListClient>(jsonString);

            for (int i = 0; i < listClient.clients.Count; i++)
            {
                City _clients = new City();
                _clients.Key = listClient.clients[i].id;
                _clients.Value = listClient.clients[i].name;

                cities.Add(_clients);
            }

            return cities;
        }

        public async Task<List<City>> GetClients()
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

            return cities;
        }
    }
}

