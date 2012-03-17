using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor.Model
{
    public class ServiceHook : IUrl, IKeyed
    {
        public long ID { get; set; }

        public string Value { get; set; }

        public string Url { get; set; }
    }
}
