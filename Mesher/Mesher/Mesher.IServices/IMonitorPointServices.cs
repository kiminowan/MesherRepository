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
        List<MonitorPoint> GetMonitorPoints();
        List<MinuteData> GetMinuteDatas(int id);
        List<Pollutant> GetPollutants();
    }
}
