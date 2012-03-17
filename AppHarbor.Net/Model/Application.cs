using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor.Model
{
    public class Application : IUrl
    {
        public Application()
        {
            // default
            this.RegionIdentitfier = "amazon-web-services::us-east-1";
        }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string RegionIdentitfier { get; set; }

        public string Url { get; set; }
    }
}
