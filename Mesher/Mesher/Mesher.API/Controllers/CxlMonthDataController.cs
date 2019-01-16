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
    [RoutePrefix("caoxiaole")]
    public class CxlMonthDataController : ApiController
    {
        
        /// <summary>
        /// 属性注入
        /// </summary>
        public ICxlMonthAnalizeServers cxlMonthAnalizeServers { get; set; }

        [Route("GetMonthAnalizeDatas")]
        [HttpGet]
        
        public List<MonthAnalizeData> GetMonthAnalizeDatas(int Code, DateTime time)
        {

            return cxlMonthAnalizeServers.GetMonthAnalizeDatas(Code,time);
        }
        [Route("GetMonitorPoints")]
        [HttpGet]

        public List<MonitorPoint> GetMonitorPoints(int Code)
        {

            return cxlMonthAnalizeServers.GetMonitorPoints(Code);
        }
    }
}
