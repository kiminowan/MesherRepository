using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mesher.MVC.Controllers
{
    public class HotController : Controller
    {
        // GET: Hot
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllIndex()
        {
            return View();
        }
    }
}