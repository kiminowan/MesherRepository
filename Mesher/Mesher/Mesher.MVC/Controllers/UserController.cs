using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mesher.MVC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        //注册页面
        public ActionResult register()
        {
            return View();
        }
        //登录页面
        public ActionResult login()
        {
            return View();
        }
    }
}