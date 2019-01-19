using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.IServices
{
    using Mesher.Entity;
    public interface ICxlMonthAnalizeServers
    {
        List<MonthAnalizeData> GetMonthAnalizeDatas(int Code, DateTime time);
        List<MonitorPoint> GetMonitorPoints(int Code);
        List<NationalControl> GetNationalControls(int Code, string pollname, string name);
        List<NationalControl> GetZh(int cor);
        List<NationalControl> GetSum(int Code, string time, string pullname);
    }
}
