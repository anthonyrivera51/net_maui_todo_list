using System;
using System.Diagnostics.Metrics;
using maui_todo_list.Model;

namespace maui_todo_list.Services
{
    public interface IClient
    {
        Task<List<City>> GetClients();
        List<City> GetClient();
    }
}

