using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppHarbor.Model;

namespace AppHarbor
{
    public static class Util
    {
        public static string GetCollaboratorType(CollaboratorType collaboratorType)
        {
            var name = Enum.GetName(typeof(CollaboratorType), collaboratorType)
                .ToLower();
            return name;
        }
    }
}
