using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetsManagment.Controllers
{
    public class LogingController : Controller
    {
        //
        // GET: /Loging/
        //public ActionResult Index()
        //{
        //    return View("Login");
        //}

        public ActionResult Login()
        {
            return View("Login");
        }
	}
}