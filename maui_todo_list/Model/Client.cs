using System;

namespace maui_todo_list.Model
{
    public class Client
    {
        public string id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public object deletedAt { get; set; }
        public string name { get; set; }
        public string numberDocument { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
    }

    public class ListClient
    {
        public List<Client> clients { get; set; }
    }

    public class ListVisited
    {
        public List<VisitType> visitTypes { get; set; }
    }

    public class VisitType
    {
        public string id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public object deletedAt { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
    }

}

