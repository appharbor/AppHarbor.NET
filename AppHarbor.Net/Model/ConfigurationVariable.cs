using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor.Model
{
    public class ConfigurationVariable : IUrl, IKeyed
    {
        public long ID { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Url { get; set; }
    }
}
