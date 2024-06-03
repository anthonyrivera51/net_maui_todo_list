using System;
using static Java.Util.Jar.Attributes;

namespace maui_todo_list.Model
{
    public class Users
    {
        public string token { get; set; }
        public string name { get; set; }
        public string identificationNumber { get; set; }
        public string id { get; set; }
        public string email { get; set; }

        public Users()
        {
        }
    }
}

