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
        List<AnalyzeEcharts> GetAnalyzeEcharts(string StartTime, string EndTime);
        //获取监测点名称
        List<MonitorPoint> GetMonitorPoints();
        //单站点多污染物
        List<AnalyzeEcharts> GetSingleSite();
    }
}
