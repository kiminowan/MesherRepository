using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mesher.MVC.Controllers
{
    using System.IO;
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
        //测试1
        public ActionResult video()
        {
            return View();
        }

        public string UploadFile()
        {
            //获取文件
            HttpPostedFileBase httpFile = Request.Files[0];
            if (httpFile != null)
            {


                //目录的单词是Dirctory   字典Dinctionary

                //获取路径
                string strPath = Server.MapPath("~/images/");
                //判断目录是否存在
                if (!Directory.Exists(strPath))
                    //如果目录不存在进行创建目录
                    Directory.CreateDirectory(strPath);
                //string newPath=strPath+"/"+httpFile.FileName;
                //获取的是全路径
                string newPath = Path.Combine(strPath, httpFile.FileName);
                httpFile.SaveAs(newPath);
                return "/images/" + httpFile.FileName;

            }
            return null;
        }
    }
}