using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.IServices
{
    using Mesher.Entity;
    public interface IAnalyzeEchartsServices
    {
        //多站点
        List<AnalyzeEcharts> GetManyPollutants(string StartTime, string EndTime, int PollutantId);
        //单站点
        List<AnalyzeEcharts> GetSingleSite(string StartTime, string EndTime, int PollutantId, int Id, string RegionCode);
        //获取监测点名称
        List<MonitorPoint> GetMonitorPoints(string RegionCode);
        //污染物
        List<AnalyzeEcharts> GetPollutant();
    }
}
