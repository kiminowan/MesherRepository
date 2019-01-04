using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    /// <summary>
    /// 实时数据表
    /// </summary>
    public class RealData
    {
        //主键ID
        public int Id { get; set; }
        //检测值
        public decimal MonitorValue { get; set; }
        //监测点污染物关联表ID
        public int Monitor_PollutionId { get; set; }
        //数据抓取时间
        public DateTime CreateTime { get; set; }
    }
}
