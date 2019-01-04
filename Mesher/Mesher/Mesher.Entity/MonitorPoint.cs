using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Entity
{
    /// <summary>
    /// 监测点表
    /// </summary>
    public class MonitorPoint
    {
        //监测点主键ID
        public int Id { get; set; }
        //监测点名称
        public string PointName { get; set; }
        //经度
        public decimal Longitude { get; set; }
        //纬度
        public decimal Latitude { get; set; }
        //设备编号
        public string EquipmentCode { get; set; }
        //监测点类型
        public int PointType { get; set; }
        //行政区编号
        public string RegionCode { get; set; }
        //状态
        public int Status { get; set; }
        //更新时间
        public DateTime UpdateDate { get; set; }
        //距离最近的国控点
        public int NearlyStation { get; set; }
    }
}
