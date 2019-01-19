using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.IServices
{
    using Mesher.Entity;
    public interface IHotService
    {
        /// <summary>
        /// 获取小时数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="polluantid"></param>
        /// <param name="monitortime"></param>
        /// <returns></returns>
        List<HourData> GetHourDatas(int id, int polluantid, string monitortime);

        /// <summary>
        /// 获取所有站点
        /// </summary>
        /// <returns></returns>
        List<MonitorPoint> GetMonitorPoints(int userid);

        /// <summary>
        /// 获取所有污染物
        /// </summary>
        /// <returns></returns>
        List<Pollutant> GetPollutants();

        /// <summary>
        /// 获取月数据
        /// </summary>
        /// <param name="polluantid"></param>
        /// <param name="monitortime"></param>
        /// <returns></returns>
        List<MonthData> GetMonthDatas(int polluantid, string monitortime, int userid);

        List<PoitInfo> GetPoitInfos();
    }
}
