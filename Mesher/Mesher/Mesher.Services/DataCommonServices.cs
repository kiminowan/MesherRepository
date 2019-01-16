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
    public class DataCommonServices:IDataCommonServices
    {
        /// <summary>
        /// 获取所有站点监测污染物关联数据
        /// </summary>
        /// <returns></returns>
        public List<MonitorPointPollutan> GetMonitorPointPollutans()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                //三表联查
                string sql = string.Format("select id, pointid, pollutantid from monitorpointpollutan ");
                var collectList = conn.Query<MonitorPointPollutan>(sql, null);
                return collectList.ToList();
            }
        }
    }
}
