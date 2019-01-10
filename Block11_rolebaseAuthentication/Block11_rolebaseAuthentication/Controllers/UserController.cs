using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Block11_rolebaseAuthentication.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}