using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.IServices
{
    using Mesher.Entity;
    public interface IMonitorPointServices
    {
        /// <summary>
        /// 显示标记点
        /// </summary>
        /// <returns></returns>
        List<MonitorPoint> GetMonitorPoints(int id);
        /// <summary>
        /// 站点排名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<MinuteData> GetMinuteDatas(int id, int userid);
        /// <summary>
        /// 获取所有的污染物
        /// </summary>
        /// <returns></returns>
        List<Pollutant> GetPollutants();
        /// <summary>
        /// 预警排名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<MinuteData> GetAqi(int id);
        /// <summary>
        /// 根据登录用户Id查找行政区
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Region> GetRegionname(int id);
    }
}
