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

        [HttpPost]
        public ActionResult Login()
        {
            string username = Request["username"];
            string password = Request["password"];
            string eingeloggtbleiben = Request["eingeloggtbleiben"];

            if ("michaeljakober".Equals(username) && "123456".Equals(password))
            {
                ViewBag.Message = "Successfully logged in!";
                if (eingeloggtbleiben == "J")
                {
                    HttpCookie cookie = new HttpCookie("Sessioncookie", "Logged in");
                    cookie.Expires = DateTime.Now.AddDays(14);
                }
                else
                {
                    Session["Logged in"] = true;
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