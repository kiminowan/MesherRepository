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
    [RoutePrefix("Mesher")]
    public class UserController : ApiController
    {
        /// <summary>
        /// 属性注入
        /// </summary>
        public IUserServices user { get; set; }
        /// <summary>
        /// 用户添加
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [Route("Add")]
        [HttpGet]
        public int Add(User users)
        {
            return user.UserAdd(users);
        }
    }
}
