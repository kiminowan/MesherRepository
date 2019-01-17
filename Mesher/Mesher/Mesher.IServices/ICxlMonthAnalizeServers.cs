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
        List<MonthAnalizeData> GetMonthAnalizeDatas(string Code);
    }
}
