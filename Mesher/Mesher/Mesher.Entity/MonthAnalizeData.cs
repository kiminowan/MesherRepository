using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    /// <summary>
    /// 月度对比数据实体
    /// </summary>
    public class MonthAnalizeData
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public double AVGValue { get; set; }
        /// <summary>
        /// 月度数据的时间
        /// </summary>
        public DateTime MonitorTime { get; set; }
        /// <summary>
        /// 监测点名称
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 污染物名称
        /// </summary>
        public string PollutantName { get; set; }

    }
}
