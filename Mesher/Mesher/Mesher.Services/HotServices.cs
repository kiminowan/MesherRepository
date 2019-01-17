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
    public class HotServices:IHotService
    {
        /// <summary>
        /// 获取小时数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="polluantid"></param>
        /// <param name="monitortime"></param>
        /// <returns></returns>
        public List<HourData> GetHourDatas(int id,int polluantid,string monitortime)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {

                string sql = @"select a.avgvalue,a.monitortime from hourdata a inner join monitorpointpollutan b on a.monitor_pollutionid=b.id inner join pollutant c on b.pollutantid=c.id inner join monitorpoint d on b.pointid=d.id where d.id=:id and c.id=:polluantid and monitortime>to_date(:monitortime,'yyyy') and monitortime<to_date(:monitortime+1,'yyyy') order by a.monitortime";
                var condition = new { Id = id,Polluantid= polluantid,Monitortime= monitortime };
                var collectList = conn.Query<HourData>(sql, condition);
                return collectList.ToList();

            }
        }

        /// <summary>
        /// 获取站点
        /// </summary>
        /// <returns></returns>
        public List<MonitorPoint> GetMonitorPoints()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = "select * from monitorpoint ";
                var collectList = conn.Query<MonitorPoint>(sql, null);
                return collectList.ToList();

            }
        }

        /// <summary>
        /// 获取所有污染物
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
        /// 获取月数据
        /// </summary>
        /// <param name="polluantid"></param>
        /// <param name="monitortime"></param>
        /// <returns></returns>
        public List<MonthData> GetMonthDatas(int polluantid,string monitortime)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {

                string sql = @"select * from monthdata a inner join monitorpointpollutan b on a.monitor_pollutionid=b.id inner join pollutant c on b.pollutantid=c.id inner join monitorpoint d on b.pointid=d.id where c.id=:polluantid and monitortime>to_date(:monitortime,'yyyy') and monitortime<to_date(:monitortime+1,'yyyy') order by a.monitortime";
                var condition = new {  Polluantid = polluantid, Monitortime = monitortime };
                var collectList = conn.Query<MonthData>(sql, condition);
                return collectList.ToList();

            }
        }


        
    }
}
