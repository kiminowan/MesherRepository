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
        public IMonitorPointServices monitorPoint { get; set; }
        [HttpGet]
        [Route("GetMonitorPoint")]
        public List<MonitorPoint> GetMonitorPoint()
        {
            var result = monitorPoint.GetMonitorPoints();

            return result;
        }
    }
}
