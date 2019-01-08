using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Services
{
    using Mesher.IServices;
    using Mesher.Entity;
    using Oracle.ManagedDataAccess.Client;
    using Dapper;
    public class RegionServices:IRegionServices
    {
        /// <summary>
        /// 获取所有行政区的名称编号
        /// </summary>
        /// <returns></returns>
        public List<Region> GetRegions()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = string.Format("select * from Region");
                var collectList = conn.Query<Region>(sql, null);
                return collectList.ToList<Region>();
            }

        }

    }
}
