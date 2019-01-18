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
        //多站点
        [HttpGet]
        [Route("GetManyPollutants")]
        public List<AnalyzeEcharts> GetManyPollutants(string StartTime, string EndTime,int PollutantId)
        {
            var result = analyzeEcharts.GetManyPollutants(StartTime, EndTime, PollutantId);
            return result;
        }
        //单站点
        [HttpGet]
        [Route("GetSingleSite")]
        public List<AnalyzeEcharts> GetSingleSite(string StartTime, string EndTime, int PollutantId,int Id,string RegionCode)
        {
            var result = analyzeEcharts.GetSingleSite(StartTime, EndTime, PollutantId, Id,RegionCode);
            return result;
        }
        //获取监测点名称
        [HttpGet]
        [Route("GetMonitorPoints")]
        public List<MonitorPoint> GetMonitorPoints( string PointName, string RegionCode)
        {
            var result = analyzeEcharts.GetMonitorPoints(RegionCode);
            if (!string.IsNullOrWhiteSpace(PointName))
            {
                result = result.Where(r => r.PointName.Contains(PointName)).ToList();
            }
            
            return result;
        }
        //单站点多污染物
        [HttpGet]
        [Route("GetPollutant")]
        public List<AnalyzeEcharts> GetPollutant()
        {
            var result = analyzeEcharts.GetPollutant();
            return result;
            
        }
    }
}
