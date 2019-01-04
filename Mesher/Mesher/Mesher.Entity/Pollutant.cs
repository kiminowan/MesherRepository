using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    /// <summary>
    /// 污染物表
    /// </summary>
    public class Pollutant
    {
        //污染物主键
        public int Id { get; set; }
        //污染物名称
        public string PollutantName { get; set; }
        //是否启用
        public int IsUsed { get; set; }
    }
}
