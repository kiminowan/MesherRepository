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
    
    public class HotController : ApiController
    {
        public IHotService hotService { get; set; }
        [HttpGet]
        [Route("GetHourDatas")]
        public List<HourData> GetHourDatas(int id, int polluantid, string monitortime)
        {
            var result = hotService.GetHourDatas(id,polluantid, monitortime);

            return result;
        }
        [HttpGet]
        [Route("GetMonitorPoints")]
        public List<MonitorPoint> GetMonitorPoints(string pointname)
        {
            List<MonitorPoint> monitorPoints= hotService.GetMonitorPoints();
            if (!string.IsNullOrWhiteSpace(pointname))
            {
                monitorPoints = monitorPoints.Where(r => r.PointName.Contains(pointname)).ToList();
            }
            return monitorPoints;
        }

        [HttpGet]
        [Route("GetPollutants")]
        public List<Pollutant> GetPollutants()
        {
            var result = hotService.GetPollutants();
            return result;
        }

        [HttpGet]
        [Route("GetMonthDatas")]
        public List<MonthData> GetMonthDatas(int polluantid, string monitortime)
        {
            var result = hotService.GetMonthDatas(polluantid, monitortime);
            return result;
        }
    }
}
