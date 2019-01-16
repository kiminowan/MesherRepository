using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    /// <summary>
    /// 用于求AQI
    /// </summary>
    public class AvgData
    {
        //监测点
        public int PointID { get; set; }
        //污染物
        public int PollutantID { get; set; }
        //均值
        public decimal AvgValue { get; set; }
    }
}
