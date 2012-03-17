using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor.Model
{
    public class Test
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Kind { get; set; }

        public TimeSpan Duration { get; set; }

        public List<Test> Tests { get; set; }
    }
}
