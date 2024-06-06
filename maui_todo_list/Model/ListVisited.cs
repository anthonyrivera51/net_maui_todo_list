using System;
namespace maui_todo_list.Model
{
    public class VisitedList
    {

        public FindAllVisitDashboard findAllVisitDashboard { get; set; }

        public class Client
        {
            public string name { get; set; }
        }

        public class Earring
        {
            public string id { get; set; }
            public string dateVisit { get; set; }
            public Client client { get; set; }
        }

        public class FindAllVisitDashboard
        {
            public List<Earring> earrings { get; set; }
            public List<object> realized { get; set; }
        }
    }
}

