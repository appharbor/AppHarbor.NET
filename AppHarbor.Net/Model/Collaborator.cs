using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor.Model
{
    public class Collaborator : IUrl, IKeyed
    {
        public long ID { get; set; }

        public UserClass User { get; set; }

        public CollaboratorType Role { get; set; }

        public string Url { get; set; }

        public class UserClass
        {
            public string ID { get; set; }

            public string Name { get; set; }
        }
    }

    public enum CollaboratorType
    {
        None = 0,
        Administrator = 1,
        Collaborator = 2
    }
}
