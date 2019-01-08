using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mesher.Entity;
using Oracle.ManagedDataAccess.Client;
using Dapper;


namespace Dataprocessing
{
    public class Minutedata
    {
        /// <summary>
        /// 数据操作
        /// </summary>
        /// <param name="valueList"></param>
        /// <returns></returns>
        public  List<DataList> DataStatistic(List<decimal> valueList)
        {
            List<DataList> Result = new List<DataList>();
            DataList dataList = new DataList();
            dataList.Avg = valueList.Average();
            dataList.Max = valueList.Max();
            dataList.Min = valueList.Min();
            Result.Add(dataList);
            return Result;
           
        }


     

    }
}
