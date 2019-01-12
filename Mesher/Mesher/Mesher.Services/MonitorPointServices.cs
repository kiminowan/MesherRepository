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
        /// 显示标记点
        /// </summary>
        /// <returns></returns>
        public List<MonitorPoint> GetMonitorPoints()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = @"select a.*,b.pointtypename from monitorpoint a inner join pointtype b on a.pointtype=b.id";
                var result = conn.Query<MonitorPoint>(sql, null);
                return result.ToList();
            }
        }
        /// <summary>
        /// 站点排名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<MinuteData> GetMinuteDatas(int id)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = @"select a.pointname,d.avgvalue from monitorpoint a inner join MonitorPointPollutan b on a.id=b.pointid
inner join Pollutant c on b.pollutantid=c.id inner join minutedata d on b.id=d.monitor_pollutionid 
where c.id=:id";
                var conditon = new { Id = id };
                var collectList = conn.Query<MinuteData>(sql, conditon);
                return collectList.ToList();
            }
        }
        /// <summary>
        /// 获取所有的污染物
        /// </summary>
        /// <returns></returns>
        public List<Pollutant> GetPollutants()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = @"select * from Pollutant";
                var result = conn.Query<Pollutant>(sql, null);
                return result.ToList();
            }
        }
        /// <summary>
        /// 预警排名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<MinuteData> GetAqi(int id)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = @"select a.pointname,d.avgvalue from monitorpoint a inner join MonitorPointPollutan b on a.id=b.pointid
inner join Pollutant c on b.pollutantid=c.id inner join minutedata d on b.id=d.monitor_pollutionid 
where c.id=id order by d.avgvalue desc";
                var conditon = new { Id = id };
                var collectList = conn.Query<MinuteData>(sql, conditon);
                return collectList.ToList();
            }
        }
        /// <summary>
        /// 根据登录用户Id查找行政区
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Region> GetRegionname(int id)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = "select b.regionname from \"User\" a inner join Region b on a.regionid=b.id where a.id=:id";
                var conditon = new { Id = id };
                var collectList = conn.Query<Region>(sql, conditon);
                return collectList.ToList();
            }

        }
    }
}
