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
    using Mesher.Services;
    using System.Security.Cryptography;
    using System.Text;

    [RoutePrefix("MonitorPoints")]
    public class MonitorPointApiController : ApiController
    {
        /// <summary>
        /// 属性注入
        /// </summary>
        public IMonitorPointServices monitorPoint { get; set; }
        /// <summary>
        /// 获取标记点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMonitorPoint")]
        public List<MonitorPoint> GetMonitorPoint(int id)
        {
            var result = monitorPoint.GetMonitorPoints(id);
            return result;
        }

        /// <summary>
        /// 获取最新分钟数据处理时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDateTime")]
        public string GetDateTime()
        {
            var result = monitorPoint.GetDateTime();
            return result.ToString();
        }

        /// <summary>
        /// 站点排名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [HttpGet]
        [Route("GetMinuteDatas")]
        public List<MinuteShowData> GetMinuteDatas(int id, int userid)
        {
            var result = monitorPoint.GetMinuteDatas(id, userid);
            return result;
        }

        /// <summary>
        /// 获取所有污染物
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPollutants")]
        public List<Pollutant> GetPollutants()
        {
            var result = monitorPoint.GetPollutants();
            return result;
        }

        /// <summary>
        /// 根据登录Id获取行政区名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRegionname")]
        public List<Region> GetRegionname(int id)
        {
            var result = monitorPoint.GetRegionname(id);
            return result;

        }


    }
}
