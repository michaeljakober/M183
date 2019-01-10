using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Block11_rolebaseAuthentication.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {
            var currentUser = (string)Session["username"];
            var userRoles = MvcApplication.userRoles;
            var currentUserRole = (string)userRoles[currentUser];

            if (currentUserRole == "Administrator")
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}