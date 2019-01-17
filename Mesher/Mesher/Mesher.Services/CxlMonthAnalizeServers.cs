using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Services
{
    using Mesher.IServices;
    using Mesher.Entity;
    using Dapper;
    using Oracle.ManagedDataAccess.Client;

    /// <summary>
    /// 月度对比分析
    /// </summary>
    public class CxlMonthAnalizeServers:ICxlMonthAnalizeServers
    {
        /// <summary>
        /// 返回月度数据（全部）
        /// </summary>
        /// <returns></returns>
        public List<MonthAnalizeData> GetMonthAnalizeDatas(string Code)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                //三表联查
                string sql = string.Format("select a.avgvalue,a.monitortime,c.PollutantName,d.PointName from MONTHDATA a ,MonitorPointPollutan b, Pollutant c,MonitorPoint d where a.Monitor_PollutionId=b.Id and b.PollutantId=c.id and b.pointid=d.id and d.RegionCode='" + Code + "'");
                var collectList = conn.Query<MonthAnalizeData>(sql, null);
                return collectList.ToList<MonthAnalizeData>();
            }
        }
    }
}
