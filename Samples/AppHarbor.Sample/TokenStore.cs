using System.Web;

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
