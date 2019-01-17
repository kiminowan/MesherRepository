using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    public  class AnalyzeEcharts
    {
        //监测点ID
        public int Id { get; set; }
        //污染物名称
        public string PollutantName { get; set; }
        //监测值
        public decimal AVGValue { get; set; }
        //监测时间
        public DateTime MonitorTime { get; set; }
        //行政区编号
        public string RegionCode { get; set; }
        //开始时间
        public DateTime StartTime { get; set; }
        //开始时间
        public DateTime EndTime { get; set; }
    }
}
