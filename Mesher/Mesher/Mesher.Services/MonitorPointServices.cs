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

    public class MonitorPointServices : IMonitorPointServices
    {
        /// <summary>
        /// 显示标记点
        /// </summary>
        /// <returns></returns>
        public List<MonitorPoint> GetMonitorPoints(int id)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {

                string sql = "select d.*,c.pointtypename from monitorpoint d inner join pointtype c on d.pointtype=c.id  where regioncode=(select b.region_code from \"User\" a inner join Region b on a.regionid=b.id where a.id=:id) ";
                var conditon = new { Id = id };
                var collectList = conn.Query<MonitorPoint>(sql, conditon);
                return collectList.ToList();

            }
        }
        /// <summary>
        /// 获取最新分钟数据处理时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTime()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = "select monitortime from minutedata order by monitortime desc ";
                var collectList = conn.Query<DateTime>(sql, null);
                return collectList.ToList().FirstOrDefault();

            }
        }
        /// <summary>
        /// 站点排名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<MinuteShowData> GetMinuteDatas(int id, int userid)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = "select monitortime from minutedata order by monitortime desc ";
                var dateTime = conn.Query<DateTime>(sql, null).ToList().FirstOrDefault();
                string sqlMinute = "select a.pointtype,a.longitude,a.latitude,a.pointname,d.avgvalue,b.pollutantid from monitorpoint a inner join MonitorPointPollutan b on a.id=b.pointid inner join minutedata d on b.id=d.monitor_pollutionid where regioncode=(select b.region_code from \"User\" a inner join Region b on a.regionid=b.id where a.id=:userid) and monitortime>=:time";
                var conditon = new { Id = id, Userid = userid, time = dateTime };
                var collectList = conn.Query<MinuteDealData>(sqlMinute, conditon);
                List<string> monitors = (from c in collectList select c.PointName).Distinct().ToList();
                List<MinuteShowData> minuteShowDatas = new List<MinuteShowData>();
                foreach (string monitor in monitors)
                {
                    MinuteShowData minuteShowData = new MinuteShowData();
                    minuteShowData.PointName = monitor;
                    
                    foreach (MinuteDealData minuteDealData in collectList)
                    {
                        if (minuteDealData.PointName == monitor)
                        {
                            minuteShowData.Latitude = minuteDealData.Latitude;
                            minuteShowData.Longitude = minuteDealData.Longitude;
                            minuteShowData.PointType = minuteDealData.PointType;
                            switch (minuteDealData.PollutantID)
                            {
                                case 11: minuteShowData.AQI = minuteDealData.AVGValue; break;
                                case 12: minuteShowData.PM2 = minuteDealData.AVGValue; break;
                                case 13: minuteShowData.CO = minuteDealData.AVGValue; break;
                                case 14: minuteShowData.NO2 = minuteDealData.AVGValue; break;
                                case 15: minuteShowData.O3 = minuteDealData.AVGValue; break;
                                case 16: minuteShowData.SO2 = minuteDealData.AVGValue; break;
                                case 17: minuteShowData.PM10 = minuteDealData.AVGValue; break;
                            }
                        }
                    }
                    minuteShowDatas.Add(minuteShowData);
                }
                return minuteShowDatas.OrderByDescending(m=>m.AQI).ToList();
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
