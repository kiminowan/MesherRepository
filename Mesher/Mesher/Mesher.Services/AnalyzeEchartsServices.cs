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

    //污染物分析Echarts图
    public class AnalyzeEchartsServices:IAnalyzeEchartsServices
    {
        /// <summary>
        /// Echarts图多站点
        /// </summary>
        /// <returns></returns>
        public List<AnalyzeEcharts> GetManyPollutants(string StartTime, string EndTime, int PollutantId)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                //monitorpoint监测点、monitorpointpollutan监测点污染物关联表、pollutant污染物、hourdata小时数据表
                string sql = @"select a.id,c.pollutantname,d.avgvalue,d.monitortime from monitorpoint a inner join monitorpointpollutan b 
on a.id=b.pointid inner join pollutant c on b.pollutantid=c.id inner join hourdata d on b.id=d.monitor_pollutionid 
where a.regioncode=110000 and c.id=:PollutantId and pointtype!=9 and a.id=138 and d.monitortime between to_date(:StartTime, 'yyyy-mm-dd ')
 and to_date(:EndTime, 'yyyy-mm-dd ') order by d.monitortime";
                var result = conn.Query<AnalyzeEcharts>(sql, new { StartTime=StartTime,EndTime=EndTime ,PollutantId=PollutantId});
                return result.ToList();
            }
        }
        /// <summary>
        /// Echarts图单站点
        /// </summary>
        /// <returns></returns>
        public List<AnalyzeEcharts> GetSingleSite(string StartTime, string EndTime, int PollutantId, int Id, string RegionCode)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                //monitorpoint监测点、monitorpointpollutan监测点污染物关联表、pollutant污染物、hourdata小时数据表
                string sql = @"select a.id,c.pollutantname,d.avgvalue,d.monitortime from monitorpoint a inner join monitorpointpollutan b 
on a.id=b.pointid inner join pollutant c on b.pollutantid=c.id inner join hourdata d on b.id=d.monitor_pollutionid 
where a.regioncode=:RegionCode and c.id=:PollutantId and pointtype!=9 and a.id=:Id and d.monitortime between to_date(:StartTime, 'yyyy-mm-dd ')
 and to_date(:EndTime, 'yyyy-mm-dd ') order by d.monitortime";
                var result = conn.Query<AnalyzeEcharts>(sql, new { StartTime = StartTime, EndTime = EndTime, PollutantId = PollutantId, Id = Id,RegionCode=RegionCode });
                return result.ToList();
            }
        }
        //获取监测点名称
        public List<MonitorPoint> GetMonitorPoints(string RegionCode)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = @"select * from monitorpoint where regioncode=:RegionCode and pointtype!=9 order by id";
                var result = conn.Query<MonitorPoint>(sql, new { RegionCode=RegionCode});
                return result.ToList();
            }
        }
        public List<AnalyzeEcharts> GetPollutant()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = @"select * from Pollutant";
                var result = conn.Query<AnalyzeEcharts>(sql, null);
                return result.ToList();
            }
        }
    }
}
