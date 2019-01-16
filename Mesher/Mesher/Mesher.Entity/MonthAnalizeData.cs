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
        /// <summary>
        /// pm2.5值
        /// </summary>
        public double PM2AVGValue { get; set; }
        /// <summary>
        /// pm10值
        /// </summary>
        public double PM10AVGValue { get; set; }
        /// <summary>
        /// 二氧化硫值
        /// </summary>
        public double SO2AVGValue { get; set; }
        /// <summary>
        /// 一氧化碳值
        /// </summary>
        public double COAVGValue { get; set; }
        /// <summary>
        /// 臭氧值
        /// </summary>
        public double O3AVGValue { get; set; }
        /// <summary>
        /// 二氧化氮值
        /// </summary>
        public double N02AVGValue { get; set; }
        /// <summary>
        /// pm2.5值
        /// </summary>
        public double TPM2AVGValue { get; set; }
        /// <summary>
        /// pm10值
        /// </summary>
        public double TPM10AVGValue { get; set; }
        /// <summary>
        /// 二氧化硫值
        /// </summary>
        public double TSO2AVGValue { get; set; }
        /// <summary>
        /// 一氧化碳值
        /// </summary>
        public double TCOAVGValue { get; set; }
        /// <summary>
        /// 臭氧值
        /// </summary>
        public double TO3AVGValue { get; set; }
        /// <summary>
        /// 二氧化氮值
        /// </summary>
        public double TN02AVGValue { get; set; }
        /// <summary>
        /// pm2.5值
        /// </summary>
        public double HPM2AVGValue { get; set; }
        /// <summary>
        /// pm10值
        /// </summary>
        public double HPM10AVGValue { get; set; }
        /// <summary>
        /// 二氧化硫值
        /// </summary>
        public double HSO2AVGValue { get; set; }
        /// <summary>
        /// 一氧化碳值
        /// </summary>
        public double HCOAVGValue { get; set; }
        /// <summary>
        /// 臭氧值
        /// </summary>
        public double HO3AVGValue { get; set; }
        /// <summary>
        /// 二氧化氮值
        /// </summary>
        public double HN02AVGValue { get; set; }

    }
}
