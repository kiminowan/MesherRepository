using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    /// <summary>
    /// 分钟数据表
    /// </summary>
    public class MinuteData
    {
        //主键ID
        public int Id { get; set; }
        //浓度均值
        public decimal AVGValue { get; set; }
        //监测点污染物关联表ID
        public int Monitor_PollutionId { get; set; }
        //浓度最大值
        public decimal MaxValue { get; set; }
        //浓度最小值
        public decimal MinValue { get; set; }
        //数据监测时间
        public DateTime MonitorTime { get; set; }
    }
}
