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
        /// <summary>
        /// 月度对比数据分析
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [Route("GetMonthAnalizeDatas")]
        [HttpGet]
        
        public List<MonthAnalizeData> GetMonthAnalizeDatas(int Code, DateTime time)
        {

            return cxlMonthAnalizeServers.GetMonthAnalizeDatas(Code,time);
        }
        /// <summary>
        /// 获取当前行政区所有的国控点
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [Route("GetMonitorPoints")]
        [HttpGet]

        public List<MonitorPoint> GetMonitorPoints(int Code)
        {

            return cxlMonthAnalizeServers.GetMonitorPoints(Code);
        }
        /// <summary>
        /// 通过传递过来的国控点的id 找到距离最近的微站点 并获取数据
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="pollname"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNationalControls")]
        public List<NationalControl> GetNationalControls(int Code, string pollname, string name)
        {
            return cxlMonthAnalizeServers.GetNationalControls(Code,pollname,name);
        }
        [HttpGet]
        [Route("GetZh")]
        /// <summary>
        /// 根据国控点的id查询距离最近的微站点
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public List<NationalControl> GetZh(int cor)
        {
            return cxlMonthAnalizeServers.GetZh(cor);
        }
    }
}
