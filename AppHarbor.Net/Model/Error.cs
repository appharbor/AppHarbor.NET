using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor.Model
{
    public class Error : IKeyed, IUrl
    {
        public long ID { get; set; }

        public string Commit_ID { get; set; }

        public DateTime Date { get; set; }

        public string Request_Path { get; set; }

        public string Message { get; set; }

        public ExceptionClass Exception { get; set; }

        public string Url { get; set; }

        public class ExceptionClass
        {
            public string Stack_Trace { get; set; }

            public string Message { get; set; }

            public string Type { get; set; }

            public string Inner_Exception { get; set; }
        }
    }
}
