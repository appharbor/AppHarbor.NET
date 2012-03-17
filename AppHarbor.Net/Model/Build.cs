using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor.Model
{
    public class Build : IUrl, IKeyed
    {
        public long ID { get; set; }

        public string Status { get; set; }

        public DateTime Created { get; set; }

        public DateTime Deployed { get; set; }

        public CommitClass Commit { get; set; }

        public string DownloadUrl { get; set; }

        public string TestsUrl { get; set; }

        public string Url { get; set; }

        public class CommitClass
        {
            public string ID { get; set; }

            public string Message { get; set; }
        }
    }
}
