using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Mesher.IServices
{
    using Entity;
    public interface IMinuteDataServices
    {
        //获取所有10分钟内实时数据
        List<RealData> GetRealDatas();
    }
}
