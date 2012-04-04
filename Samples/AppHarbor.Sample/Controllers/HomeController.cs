using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppHarbor.Sample.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.HasToken = (TokenStore.AccessToken != null);
            ViewBag.Token = TokenStore.AccessToken;
            ViewBag.AuthLink = string.Format("https://appharbor.com/user/authorizations/new?client_id={0}&redirect_uri={1}", Config.ClientID, Config.ClientCallbackUrl);

            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Auth(string code)
        {
            var clientID = Config.ClientID;
            var clientSecret = Config.ClientSecret;
            
            AuthInfo authInfo = null;
            try 
	        {	        
		        authInfo = AppHarborApi.GetAuthInfo(Config.ClientID, Config.ClientSecret, code);
	        }
	        catch (Exception)
	        {
                throw;
	        }

            if (authInfo != null)
            {
                TokenStore.AccessToken = authInfo.AccessToken;
            }

            return RedirectToAction("Index");
        }
    }
}

