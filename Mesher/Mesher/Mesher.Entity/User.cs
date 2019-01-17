using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User
    {
        //用户表ID
        public int Id { get; set; }
        //账号
        public string UserName { get; set; }
        //密码
        public string UserPassWord { get; set; }
        //用户头像
        public string UserImage { get; set; }
        //令牌
        public string Token { get; set; }
        //行政区主键
        public int RegionId { get; set; }
    }
}
