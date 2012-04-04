using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace AppHarbor.Sample
{
    public static class TokenStore
    {
        public static string AccessToken
        {
            get { return HttpContext.Current.Session["ACCESS_TOKEN"] as string; }
            set { HttpContext.Current.Session["ACCESS_TOKEN"] = value; }
        }
    }
}
