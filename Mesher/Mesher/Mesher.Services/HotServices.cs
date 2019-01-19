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

                string sql = "select a.avgvalue,a.monitortime from hourdata a inner join monitorpointpollutan b on a.monitor_pollutionid=b.id inner join pollutant c on b.pollutantid=c.id inner join monitorpoint d on b.pointid=d.id where d.id=:id and c.id=:polluantid  and monitortime>to_date(:monitortime,'yyyy') and monitortime<to_date(:monitortime+1,'yyyy') order by a.monitortime";
                var condition = new { Id = id,Polluantid= polluantid,Monitortime= monitortime };
                var collectList = conn.Query<HourData>(sql, condition);
                return collectList.ToList();

            }
        }

        /// <summary>
        /// 获取站点
        /// </summary>
        /// <returns></returns>
        public List<MonitorPoint> GetMonitorPoints(int userid)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = "select a.id,a.pointname from monitorpoint a inner join region b on a.regioncode=b.region_code inner join \"User\" c on b.id=c.regionid where c.id=:userid ";
                var condition = new { Userid=userid };
                var collectList = conn.Query<MonitorPoint>(sql, condition);
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
        public List<MonthData> GetMonthDatas(int polluantid,string monitortime,int userid)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                if (polluantid == 0)
                {
                    string sql = "select a.avgvalue,a.monitortime,d.pointname from monthdata a inner join monitorpointpollutan b on a.monitor_pollutionid=b.id inner join pollutant c on b.pollutantid=c.id inner join monitorpoint d on b.pointid=d.id inner join region e on d.regioncode=e.region_code inner join \"User\" f on e.id=f.regionid  where  f.id=:userid and monitortime>=to_date(:monitortime,'yyyy') and monitortime<to_date(:monitortime+1,'yyyy') order by a.monitortime";
                    var condition = new { Monitortime = monitortime,Userid=userid };
                    var collectList = conn.Query<MonthData>(sql, condition);
                    return collectList.ToList();
                }
                else if (monitortime == null)
                {
                    string sql = "select a.avgvalue,a.monitortime,d.pointname from monthdata a inner join monitorpointpollutan b on a.monitor_pollutionid=b.id inner join pollutant c on b.pollutantid=c.id inner join monitorpoint d on b.pointid=d.id inner join region e on d.regioncode=e.region_code inner join \"User\" f on e.id=f.regionid  where c.id=:polluantid and f.id=:userid ";
                    var condition = new { Polluantid = polluantid,Userid=userid };
                    var collectList = conn.Query<MonthData>(sql, condition);
                    return collectList.ToList();
                }
                else
                {
                    string sql = "select a.avgvalue,a.monitortime,d.pointname from monthdata a inner join monitorpointpollutan b on a.monitor_pollutionid=b.id inner join pollutant c on b.pollutantid=c.id inner join monitorpoint d on b.pointid=d.id inner join region e on d.regioncode=e.region_code inner join \"User\" f on e.id=f.regionid  where c.id=:polluantid and f.id=:userid and monitortime>=to_date(:monitortime,'yyyy') and monitortime<to_date(:monitortime+1,'yyyy') order by a.monitortime";
                    var condition = new { Polluantid = polluantid, Monitortime = monitortime,Userid=userid };
                    var collectList = conn.Query<MonthData>(sql, condition);
                    return collectList.ToList();
                }

                //string sql = @"select a.avgvalue,a.monitortime from monthdata a inner join monitorpointpollutan b on a.monitor_pollutionid=b.id inner join pollutant c on b.pollutantid=c.id inner join monitorpoint d on b.pointid=d.id where c.id=:polluantid and monitortime>to_date(:monitortime,'yyyy') and monitortime<to_date(:monitortime+1,'yyyy') order by a.monitortime";
                //var condition = new {  Polluantid = polluantid, Monitortime = monitortime };
                //var collectList = conn.Query<MonthData>(sql, condition);
                //return collectList.ToList();

            }
        }

        public List<PoitInfo> GetPoitInfos(int userid)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = "select a.pointname,c.pointtypename,b.regionname,a.updatedate from monitorpoint a inner join region b on a.regioncode=b.region_code inner join pointtype c on a.pointtype=c.id inner join \"User\" d on b.id=d.regionid where d.id=:userid";
                var condtion = new { Userid = userid };
                var result = conn.Query<PoitInfo>(sql, condtion);
                return result.ToList();
            }
        }


        
    }
}
