﻿using System;
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
    }
}
