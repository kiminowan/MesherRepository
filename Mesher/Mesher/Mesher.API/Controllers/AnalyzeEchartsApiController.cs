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
    [RoutePrefix("AnalyzeEcharts")]

    public class AnalyzeEchartsApiController : ApiController
    {
        public IAnalyzeEchartsServices analyzeEcharts { get; set; }

        [HttpGet]
        [Route("GetAnalyzeEcharts")]
        public List<AnalyzeEcharts> GetAnalyzeEcharts(string StartTime, string EndTime,int PollutantId)
        {
            var result = analyzeEcharts.GetAnalyzeEcharts(StartTime, EndTime, PollutantId);
            return result;
        }
        //获取监测点名称
        [HttpGet]
        [Route("GetMonitorPoints")]
        public List<MonitorPoint> GetMonitorPoints( string PointName)
        {
            var result = analyzeEcharts.GetMonitorPoints();
            if (!string.IsNullOrWhiteSpace(PointName))
            {
                result = result.Where(r => r.PointName.Contains(PointName)).ToList();
            }
            
            return result;
        }
        //单站点多污染物
        [HttpGet]
        [Route("GetSingleSite")]
        public List<AnalyzeEcharts> GetSingleSite()
        {
            var result = analyzeEcharts.GetSingleSite();
            return result;
            
        }
    }
}
