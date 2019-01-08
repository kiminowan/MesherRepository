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
    public class MonitorPointServices:IMonitorPointServices
    {
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public List<MonitorPoint> GetMonitorPoints()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = string.Format("select * from MonitorPoint");
                var collectList = conn.Query<MonitorPoint>(sql, null);
                return collectList.ToList<MonitorPoint>();
            }
        }

        public List<MinuteData> GetMinuteDatas(int id)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = @"select a.pointname,d.avgvalue from monitorpoint a inner join MonitorPointPollutan b on a.id=b.pointid
inner join Pollutant c on b.pollutantid=c.id inner join minutedata d on b.id=d.monitor_pollutionid 
where c.id=:id  order by d.MonitorTime desc";
                var conditon = new { Id = id };
                var collectList = conn.Query<MinuteData>(sql, conditon);
                return collectList.ToList();
            }
        }

        public List<Pollutant> GetPollutants()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = @"select * from Pollutant";
                var result = conn.Query<Pollutant>(sql, null);
                return result.ToList();
            }
        }
    }
}
