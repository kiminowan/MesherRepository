using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mesher.API.Controllers
{
    using Mesher.Entity;
    using Mesher.IServices;
    [RoutePrefix("MonitorPoints")]
    public class CxlMonthDataController : ApiController
    {
        
        /// <summary>
        /// 属性注入
        /// </summary>
        public ICxlMonthAnalizeServers cxlMonthAnalizeServers { get; set; }
        [HttpPost]
        [Route("GetMonthAnalizeDatas")]
        public List<MonthAnalizeData> GetMonthAnalizeDatas(string Code)
        {

            return cxlMonthAnalizeServers.GetMonthAnalizeDatas(Code);
        }
    }
}
