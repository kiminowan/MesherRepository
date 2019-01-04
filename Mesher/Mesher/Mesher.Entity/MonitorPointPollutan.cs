using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    /// <summary>
    /// 监测点污染物关联表
    /// </summary>
    public class MonitorPointPollutan
    {
        //监测点污染物主键
        public int Id { get; set; }
        //监测点主键
        public int PointId { get; set; }
        //污染物主键
        public int PollutantId { get; set; }
    }
}
