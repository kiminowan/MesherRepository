using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    /// <summary>
    /// 监测点类型
    /// </summary>
    public class PointType
    {
        //监测点类型主键ID
        public int Id { get; set; }
        //监测点类型名称
        public string PointTypeName { get; set; }
        //更新时间
        public DateTime UpdateDate { get; set; }
    }
}
