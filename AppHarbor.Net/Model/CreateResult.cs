using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor.Model
{
    public class CreateResult<T>
    {
        public CreateStatus Status { get; set; }

        public T ID { get; set; }

        public string Location { get; set; }
    }

    public enum CreateStatus
    {
        None = 0,
        Created = 1,
        AlreadyExists = 2,
        Undefined = 3
    }

    public class CreateResultLongID : CreateResult<long>
    {
    }

    public class CreateResultStringID : CreateResult<string>
    {
    }
}
