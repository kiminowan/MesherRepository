using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.IServices
{
    using Mesher.Entity;
    public interface IDataCommonServices
    {
        /// <summary>
        /// 获取所有站点监测污染物关联数据
        /// </summary>
        /// <returns></returns>
        List<MonitorPointPollutan> GetMonitorPointPollutans();
    }
}
