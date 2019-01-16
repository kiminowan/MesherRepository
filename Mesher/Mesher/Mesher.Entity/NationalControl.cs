using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    public class NationalControl
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string AvgValues { get; set; }
        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 微站名称
        /// </summary>
        public string PoninName { get; set; }
        /// <summary>
        /// 污染物名称
        /// </summary>
        public string PullutantName { get; set; }
    }
}
