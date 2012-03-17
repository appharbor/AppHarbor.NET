using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor.Model
{
    public class User
    {
        public string ID { get; set; }

        public string Username { get; set; }

        public List<string> Email_Addresses { get; set; }
    }
}
