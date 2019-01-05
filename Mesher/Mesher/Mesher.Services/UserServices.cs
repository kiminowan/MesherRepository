using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Services
{
    using Mesher.IServices;
    using Mesher.Common;
    using Oracle.ManagedDataAccess.Client;
    using Oracle.ManagedDataAccess;
    using Mesher.Entity;
    using Dapper;
    public class UserServices : IUserServices
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int UserAdd(User user)
        {

            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                conn.Open();
                string sql = "insert into \"User\"(Username,Userpassword,userimage,token,regionid) values('"+user.UserName+"','"+user.UserPassWord+"','"+user.UserImage+"','"+user.Token+"','"+user.RegionId+"')";
                int result = conn.Execute(sql, user);
                return result;
            }

        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = string.Format("select * from \"User\"");
                var collectList = conn.Query<User>(sql,null);
                return collectList.ToList<User>();
            }
        }
        /// <summary>
        /// 根据账号密码进行等录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public List<User> GetUsersByname(string name,string pass)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = string.Format("select * from \"User\" where Username='"+name+"' and Userpassword='"+pass+"'");
                var collectList = conn.Query<User>(sql, null);
                return collectList.ToList<User>();
            }
        }
    }
}
