using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    /// <summary>
    /// 预警表
    /// </summary>
    public class Warning
    {
        //预警主键ID
        public int Id { get; set; }
        //超标指数
        public int OverExponent { get; set; }
        //污染物外键
        public int PollutionId { get; set; }
        //微站监测点外键
        public int TinyMonitorId { get; set; }
        //国控站监测点外键
        public int StationMonitorId { get; set; }
    }
}
