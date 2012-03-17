using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor.Model
{
    public interface IKeyed
    {
        long ID { get; set; }
    }
}
