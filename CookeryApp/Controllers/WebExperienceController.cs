using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CookeryApp.Controllers
{
    public class WebExperienceController : Controller
    {
        // GET: WexExperience
        public ActionResult Index()
        {
            //cria credencial e acesso para paypal api
            var apiContext = GetApiContext();

            //obtém webprofile, cria experiance profile para colocar no payment object
            var list = WebProfile.GetList(apiContext);

            if (!list.Any())
            {
                SeedWebProfile(apiContext);
                list = WebProfile.GetList(apiContext);  
            }

            return View(list);
        }

        private void SeedWebProfile(APIContext apiContext)
        {
            var BigGods = new WebProfile()
            {
                name = "Ninja",
                input_fields = new InputFields()
                {
                    no_shipping = 1
                }
            };
            WebProfile.Create(apiContext, BigGods);
        }

        private APIContext GetApiContext()
        {
            // Authenticate with PayPal
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;
        }
    }
}