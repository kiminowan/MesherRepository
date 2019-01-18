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
    /// <summary>
    /// 分钟数据表
    /// </summary>
    public class MinuteDealData
    {
        public int ID { get; set; }
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
        //监测点名称
        public string PointName { get; set; }
        //污染物ID
        public int PollutantID { get; set; }
        //经度
        public decimal Longitude { get; set; }
        //纬度
        public decimal Latitude { get; set; }
        public int PointType { get; set; }
    }
    public class MinuteShowData
    {
        public string PointName { get; set; }
        public decimal AQI { get; set; }
        public decimal PM2 { get; set; }
        public decimal PM10 { get; set; }
        public decimal CO { get; set; }
        public decimal NO2 { get; set; }
        public decimal O3 { get; set; }
        public decimal SO2 { get; set; }
        //经度
        public decimal Longitude { get; set; }
        //纬度
        public decimal Latitude { get; set; }
        public int PointType { get; set; }
    }
}
