using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Block7_Session.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            string username = Request["username"];
            string password = Request["password"];
            string eingeloggtbleiben = Request["eingeloggtbleiben"];

            if ("michaeljakober".Equals(username) && "123456".Equals(password))
            {
                ViewBag.Message = "Successfully logged in!";
                if (eingeloggtbleiben == "true")
                {
                    FormsAuthentication.SetAuthCookie(username, true);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                }
            }
            else
            {
                ViewBag.Message = "Bad Credentials!";
            }
            return View();
        }
    }
}