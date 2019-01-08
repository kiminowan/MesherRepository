using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mesher.API.Controllers
{
    using Mesher.Entity;
    using Mesher.IServices;
    using Mesher.Services;
    using System.Security.Cryptography;
    using System.Text;

    [RoutePrefix("Mesher")]
    public class UserController : ApiController
    {
        /// <summary>
        /// 属性注入
        /// </summary>
        public IUserServices user { get; set; }
        /// <summary>
        /// 属性注入 行政区
        /// </summary>
        public IRegionServices Region { get; set; }
        /// <summary>
        /// 用户添加
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public int Add(User users)
        {
            string yonghuming = users.UserName;
            string pass=users.UserPassWord;
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(pass)), 4, 8);
            t2 = t2.Replace("-", "");
            users.UserPassWord = t2;            
            List<User> i=user.GetUsers().Where(n=>n.UserName.Equals(yonghuming)).ToList();
            if (i.Count>=1)
            {
                return 2;
            }
            else {
                return user.UserAdd(users);
            }
        }
        [Route("LoginByNamePass")]
        [HttpPost]
        /// <summary>
        /// 根据登陆的用户名，密码进行判断
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public int LoginByNamePass(User users)
        {
            string yname = users.UserName;
            string ypass = users.UserPassWord;
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(ypass)), 4, 8);
            t2 = t2.Replace("-", "");
            List<User> get = user.GetUsersByname(yname, t2);
            if (get.Count>0)
            {
                return 1;
            }
            else {
                return 0;
            }

        }
        [Route("GetRegions")]
        /// <summary>
        /// 获取行政区的编号 名称
        /// </summary>
        /// <returns></returns>
        public List<Region> GetRegions()
        {
            return Region.GetRegions();
        }
    }
}
