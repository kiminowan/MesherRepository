using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mesher.Entity;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using Dataprocessing;

namespace Mesher.Services
{
    public class FirstPage
    {
        /// <summary>
        /// 分钟数据
        /// </summary>
        /// <returns></returns>
        public List<DataList> GetMinute()
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {

                string sql = @"select d.MonitorValue from MonitorPoint a inner join MonitorPointPollutan b on a.id=b.pointid inner join Pollutant c on b.pollutantid=c.id
   inner join RealData d on d.monitor_pollutionid=b.id where createtime>(sysdate-numtodsinterval(10,'minute')); ";
                //var conditon = new { PollutantName = pollutantname };
                var result = conn.Query<RealData>(sql, null);
                List<decimal> valueList = new List<decimal>();
                foreach (var data in result)
                {
                    valueList.Add(data.MonitorValue);
                    
                }

                Minutedata minutedata = new Minutedata();
                var Avgresult= minutedata.DataStatistic(valueList) ;
                return Avgresult;


            }
        }

        /// <summary>
        /// 小时数据
        /// </summary>
        /// <returns></returns>
        public List<DataList> Gethour()
        {
        
            List<decimal> valueList = new List<decimal>();
            var result = GetMinute();
            foreach (var data in result)
            {
                valueList.Add(data.Avg);
            }
            Minutedata minutedata = new Minutedata();
            var Avgresult = minutedata.DataStatistic(valueList);
            return Avgresult;

        }

        /// <summary>
        /// 日数据
        /// </summary>
        /// <returns></returns>
        public List<DataList> Getday()
        {
            List<decimal> valueList = new List<decimal>();
            var result = Gethour();
            foreach (var data in result)
            {
                valueList.Add(data.Avg);
            }
            Minutedata minutedata = new Minutedata();
            var Avgresult = minutedata.DataStatistic(valueList);
            return Avgresult;
        }
  
 
    }
}
