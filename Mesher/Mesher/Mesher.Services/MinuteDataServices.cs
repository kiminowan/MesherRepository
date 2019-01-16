using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Mesher.Services
{
    using IServices;
    using Entity;
    using Oracle.ManagedDataAccess.Client;
    using Dapper;
    public class MinuteDataServices:IMinuteDataServices
    {
        //获取所有10分钟内实时数据
        public List<RealData> GetRealDatas()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = @"select * from realdata r join monitorpointpollutan mp on r.monitor_pollutionid=mp.id where r.createtime> sysdate+numtodsinterval(-10,'minute')";
                var result = conn.Query<RealData>(sql, null);
                return result.ToList();
            }
        }
    }
}
