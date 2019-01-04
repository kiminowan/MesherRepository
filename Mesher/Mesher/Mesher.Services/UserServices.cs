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

    }
}
