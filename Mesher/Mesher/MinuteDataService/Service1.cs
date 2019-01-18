using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace MinuteDataService
{
    using Mesher.Entity;
    using Mesher.IServices;
    using Mesher.Services;
    using Oracle.ManagedDataAccess.Client;
    using Dapper;
    using Newtonsoft.Json;

    public partial class Service1 : ServiceBase
    {
        private const String host = "https://api.epmap.org";
        private const String path = "/api/v1/air/station";
        private const String method = "GET";
        private const String appcode = "159e9fc8a3ac482ea93c8140cd9fdace";
        System.Timers.Timer timer1;
        System.Timers.Timer timer2;
        System.Timers.Timer timer3;
        System.Timers.Timer timer4;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new System.Timers.Timer();
            timer1.Interval = 1000;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            timer1.Enabled = true;
            timer2 = new System.Timers.Timer();
            timer2.Interval = 60000; // 600000*6;
            timer2.Elapsed += new System.Timers.ElapsedEventHandler(timer2_Elapsed);
            timer2.Enabled = true;
            timer3 = new System.Timers.Timer();
            timer3.Interval = 60000 * 60;
            timer3.Elapsed += new System.Timers.ElapsedEventHandler(timer3_Elapsed);
            timer3.Enabled = true;
            timer4 = new System.Timers.Timer();
            timer4.Interval = 60000 * 60 * 24;
            timer4.Elapsed += new System.Timers.ElapsedEventHandler(timer4_Elapsed);
            timer4.Enabled = true;
        }

        protected override void OnStop()
        {
        }
        #region 十分钟数据
        /// <summary>
        /// 十分钟数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute % 10 == 0 && DateTime.Now.Second == 0)
            {
                //取得十分钟内实时数据
                List<RealData> realDatas = GetRealDatas();
                if (realDatas.Count == 0)
                {
                    return;
                }
                //获取监测点污染物
                List<MonitorPointPollutan> monitorPointPollutans = GetMonitorPointPollutans();
                //用于数据库中十分钟浓度的添加

                List<MinuteData> minuteDatas = new List<MinuteData>();
                //用于求十分钟的AQI

                List<AvgData> minuteAvgDatas = new List<AvgData>();
                DateTime dateTime = DateTime.Now;
                List<HourData> hourDatas = GetLastHourDatas();
                foreach(HourData hourData in hourDatas)
                {
                    MinuteData minuteData = new MinuteData();
                    minuteData.AVGValue = hourData.AVGValue;
                    minuteData.Monitor_PollutionId = hourData.Monitor_PollutionId;
                    minuteData.MonitorTime = dateTime;
                    minuteDatas.Add(minuteData);
                }
                //循环遍历除AQI之外的监测点污染物
                foreach (MonitorPointPollutan monitorPointPollutan in monitorPointPollutans.Where(m => m.PollutantId != 11))
                {
                    MinuteData minuteData = new MinuteData();
                    AvgData minuteAvgData = new AvgData();
                    var mpRealDatas = realDatas.Where(m => m.Monitor_PollutionId == monitorPointPollutan.Id);

                    minuteData.Monitor_PollutionId = monitorPointPollutan.Id;
                    minuteData.MaxValue = mpRealDatas.Max(m => m.MonitorValue);
                    minuteData.MinValue = mpRealDatas.Min(m => m.MonitorValue);
                    minuteData.AVGValue = mpRealDatas.Average(m => m.MonitorValue);
                    minuteData.MonitorTime = dateTime;
                    minuteDatas.Add(minuteData);

                    minuteAvgData.AvgValue = mpRealDatas.Average(m => m.MonitorValue);
                    minuteAvgData.PointID = monitorPointPollutan.PointId;
                    minuteAvgData.PollutantID = monitorPointPollutan.PollutantId;
                    minuteAvgDatas.Add(minuteAvgData);
                }
                AQICompute aqiCompute = new AQICompute();
                foreach (MonitorPointPollutan monitorPointPollutan in monitorPointPollutans.Where(m => m.PollutantId == 11))
                {
                    MinuteData minuteData = new MinuteData();
                    minuteData.Monitor_PollutionId = monitorPointPollutan.Id;
                    minuteData.MonitorTime = dateTime;
                    decimal pm2 = 0;
                    decimal pm10 = 0;
                    decimal co = 0;
                    decimal no2 = 0;
                    decimal o3 = 0;
                    decimal so2 = 0;
                    foreach (AvgData avgData in minuteAvgDatas)
                    {
                        if (avgData.PointID == monitorPointPollutan.PointId)
                        {
                            switch (avgData.PollutantID)
                            {
                                case 12: pm2 = avgData.AvgValue; break;
                                case 13: co = avgData.AvgValue; break;
                                case 14: no2 = avgData.AvgValue; break;
                                case 15: o3 = avgData.AvgValue; break;
                                case 16: so2 = avgData.AvgValue; break;
                                case 17: pm10 = avgData.AvgValue; break;
                            }
                        }
                    }
                    minuteData.AVGValue = Convert.ToDecimal(aqiCompute.GetAQI(pm2, co, no2, o3, so2, pm10));
                    minuteDatas.Add(minuteData);
                }
                AddMinuteData(minuteDatas);
            }
        }
        private List<HourData> GetLastHourDatas()
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = "select monitortime from hourdata order by monitortime desc ";
                var dateTime = conn.Query<DateTime>(sql, null).ToList().FirstOrDefault();
                var hourDatas = conn.Query<HourData>("select h.* from hourdata h join monitorpointpollutan mp on h.monitor_pollutionid=mp.id join monitorpoint m on mp.pointid=m.id where monitortime>=:time and m.pointtype=9", new { time = dateTime });
                if (hourDatas.Count() > 0)
                {
                    return hourDatas.ToList();
                }
                return null;
            }
        }

        /// <summary>
        /// 获取所有10分钟内实时数据
        /// </summary>
        /// <returns></returns>
        private List<RealData> GetRealDatas()
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = @"select * from realdata r join monitorpointpollutan mp on r.monitor_pollutionid=mp.id where r.createtime> sysdate+numtodsinterval(-10,'minute')";
                var result = conn.Query<RealData>(sql, null);
                return result.ToList();
            }
        }
        //进行分钟数据的添加
        private void AddMinuteData(List<MinuteData> minuteDatas)
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = @"insert into minutedata  ( avgvalue, monitor_pollutionid, maxvalue, minvalue, monitortime) values  ( :AVGValue, :Monitor_PollutionId, :MaxValue, :MinValue, :MonitorTime)";
                conn.Execute(sql, minuteDatas);
            }
        }
        #endregion
        #region 一小时数据
        private void timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute == 0)
            {
                //取得一小时内分钟数据
                List<MinuteData> minuteDatas = GetMinuteDatas();
                if (minuteDatas.Count == 0)
                {
                    return;
                }
                //获取监测点污染物
                List<MonitorPointPollutan> monitorPointPollutans = GetMonitorPointPollutans();
                //用于数据库中一小时浓度的添加

                List<HourData> hourDatas = new List<HourData>();
                //用于求一小时的AQI

                List<AvgData> avgDatas = new List<AvgData>();
                DateTime dateTime = DateTime.Now;
                //循环遍历除AQI之外的监测点污染物
                foreach (MonitorPointPollutan monitorPointPollutan in monitorPointPollutans.Where(m => m.PollutantId != 11))
                {
                    HourData hourData = new HourData();
                    AvgData avgData = new AvgData();
                    var mpMinuteDatas = minuteDatas.Where(m => m.Monitor_PollutionId == monitorPointPollutan.Id);

                    hourData.Monitor_PollutionId = monitorPointPollutan.Id;
                    hourData.MaxValue = mpMinuteDatas.Max(m => m.AVGValue);
                    hourData.MinValue = mpMinuteDatas.Min(m => m.AVGValue);
                    hourData.AVGValue = mpMinuteDatas.Average(m => m.AVGValue);
                    hourData.MonitorTime = dateTime;
                    hourDatas.Add(hourData);

                    avgData.AvgValue = mpMinuteDatas.Average(m => m.AVGValue);
                    avgData.PointID = monitorPointPollutan.PointId;
                    avgData.PollutantID = monitorPointPollutan.PollutantId;
                    avgDatas.Add(avgData);
                }
                AQICompute aqiCompute = new AQICompute();
                foreach (MonitorPointPollutan monitorPointPollutan in monitorPointPollutans.Where(m => m.PollutantId == 11))
                {
                    HourData hourData = new HourData();
                    hourData.Monitor_PollutionId = monitorPointPollutan.Id;
                    hourData.MonitorTime = dateTime;
                    decimal pm2 = 0;
                    decimal pm10 = 0;
                    decimal co = 0;
                    decimal no2 = 0;
                    decimal o3 = 0;
                    decimal so2 = 0;
                    foreach (AvgData avgData in avgDatas)
                    {
                        if (avgData.PointID == monitorPointPollutan.PointId)
                        {
                            switch (avgData.PollutantID)
                            {
                                case 12: pm2 = avgData.AvgValue; break;
                                case 13: co = avgData.AvgValue; break;
                                case 14: no2 = avgData.AvgValue; break;
                                case 15: o3 = avgData.AvgValue; break;
                                case 16: so2 = avgData.AvgValue; break;
                                case 17: pm10 = avgData.AvgValue; break;
                            }
                        }
                    }
                    hourData.AVGValue = Convert.ToDecimal(aqiCompute.GetAQI(pm2, co, no2, o3, so2, pm10));
                    hourDatas.Add(hourData);
                }
                #region 国控站
                string stations = "[{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"万寿西宫\",\"station_code\":\"1001A\",\"lng\":116.366,\"lat\":39.8673},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"定陵\",\"station_code\":\"1002A\",\"lng\":116.17,\"lat\":40.2865},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"东四\",\"station_code\":\"1003A\",\"lng\":116.434,\"lat\":39.9522},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"天坛\",\"station_code\":\"1004A\",\"lng\":116.434,\"lat\":39.8745},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"农展馆\",\"station_code\":\"1005A\",\"lng\":116.473,\"lat\":39.9716},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"官园\",\"station_code\":\"1006A\",\"lng\":116.361,\"lat\":39.9425},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"海淀区万柳\",\"station_code\":\"1007A\",\"lng\":116.315,\"lat\":39.9934},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"顺义新城\",\"station_code\":\"1008A\",\"lng\":116.72,\"lat\":40.1438},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"怀柔镇\",\"station_code\":\"1009A\",\"lng\":116.644,\"lat\":40.3937},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"昌平镇\",\"station_code\":\"1010A\",\"lng\":116.23,\"lat\":40.1952},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"奥体中心\",\"station_code\":\"1011A\",\"lng\":116.407,\"lat\":40.0031},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"古城\",\"station_code\":\"1012A\",\"lng\":116.225,\"lat\":39.9279},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"勤俭道\",\"station_code\":\"1015A\",\"lng\":117.145,\"lat\":39.1654},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"大直沽八号路\",\"station_code\":\"1017A\",\"lng\":117.237,\"lat\":39.1082},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"前进道\",\"station_code\":\"1018A\",\"lng\":117.202,\"lat\":39.0927},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"淮河道\",\"station_code\":\"1019A\",\"lng\":117.1837,\"lat\":39.2133},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"跃进路\",\"station_code\":\"1021A\",\"lng\":117.307,\"lat\":39.0877},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"第四大街\",\"station_code\":\"1023A\",\"lng\":117.707,\"lat\":39.0343},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"永明路\",\"station_code\":\"1024A\",\"lng\":117.457,\"lat\":38.8394},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"汉北路\",\"station_code\":\"1026A\",\"lng\":117.764,\"lat\":39.1587},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"团泊洼\",\"station_code\":\"1027A\",\"lng\":117.157,\"lat\":38.9194},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"河西一经路\",\"station_code\":\"2858A\",\"lng\":117.7918,\"lat\":39.2474},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"津沽路\",\"station_code\":\"2859A\",\"lng\":117.3747,\"lat\":38.9846},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"宾水西道\",\"station_code\":\"2860A\",\"lng\":117.1589,\"lat\":39.0845},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"大理道\",\"station_code\":\"2922A\",\"lng\":117.1941,\"lat\":39.1067},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"中山北路\",\"station_code\":\"3020A\",\"lng\":117.2099,\"lat\":39.16969},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"西四道\",\"station_code\":\"3051A\",\"lng\":117.3916,\"lat\":39.1495},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"职工医院\",\"station_code\":\"1029A\",\"lng\":114.4548,\"lat\":38.0513},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"高新区\",\"station_code\":\"1030A\",\"lng\":114.6046,\"lat\":38.0398},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"西北水源\",\"station_code\":\"1031A\",\"lng\":114.5019,\"lat\":38.1398},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"西南高教\",\"station_code\":\"1032A\",\"lng\":114.4586111,\"lat\":38.00583333},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"世纪公园\",\"station_code\":\"1033A\",\"lng\":114.5330556,\"lat\":38.01777778},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"人民会堂\",\"station_code\":\"1034A\",\"lng\":114.5214,\"lat\":38.0524},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"封龙山\",\"station_code\":\"1035A\",\"lng\":114.3541,\"lat\":37.9097},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"22中南校区\",\"station_code\":\"2862A\",\"lng\":114.5480556,\"lat\":38.03777778},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"供销社\",\"station_code\":\"1036A\",\"lng\":118.1662,\"lat\":39.6308},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"雷达站\",\"station_code\":\"1037A\",\"lng\":118.144,\"lat\":39.643},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"物资局\",\"station_code\":\"1038A\",\"lng\":118.1853,\"lat\":39.6407},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"陶瓷公司\",\"station_code\":\"1039A\",\"lng\":118.2185,\"lat\":39.6679},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"十二中\",\"station_code\":\"1040A\",\"lng\":118.1838,\"lat\":39.65782},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"小山\",\"station_code\":\"1041A\",\"lng\":118.1997,\"lat\":39.6295},{\"city\":\"秦皇岛市\",\"city_code\":\"130300\",\"station\":\"北戴河环保局\",\"station_code\":\"1042A\",\"lng\":119.5259,\"lat\":39.8283},{\"city\":\"秦皇岛市\",\"city_code\":\"130300\",\"station\":\"第一关\",\"station_code\":\"1043A\",\"lng\":119.7624,\"lat\":40.0181},{\"city\":\"秦皇岛市\",\"city_code\":\"130300\",\"station\":\"监测站\",\"station_code\":\"1044A\",\"lng\":119.6023,\"lat\":39.9567},{\"city\":\"秦皇岛市\",\"city_code\":\"130300\",\"station\":\"建设大厦\",\"station_code\":\"1046A\",\"lng\":119.5369,\"lat\":39.9419},{\"city\":\"秦皇岛市\",\"city_code\":\"130300\",\"station\":\"文明里(启用171012)\",\"station_code\":\"3132A\",\"lng\":119.5967,\"lat\":37.0308},{\"city\":\"邯郸市\",\"city_code\":\"130400\",\"station\":\"环保局\",\"station_code\":\"1047A\",\"lng\":114.5129,\"lat\":36.61763},{\"city\":\"邯郸市\",\"city_code\":\"130400\",\"station\":\"东污水处理厂\",\"station_code\":\"1048A\",\"lng\":114.5426,\"lat\":36.6164},{\"city\":\"邯郸市\",\"city_code\":\"130400\",\"station\":\"矿院\",\"station_code\":\"1049A\",\"lng\":114.5035,\"lat\":36.5776},{\"city\":\"邯郸市\",\"city_code\":\"130400\",\"station\":\"丛台公园\",\"station_code\":\"1050A\",\"lng\":114.4965,\"lat\":36.61981},{\"city\":\"邢台市\",\"city_code\":\"130500\",\"station\":\"达活泉\",\"station_code\":\"1077A\",\"lng\":114.4821,\"lat\":37.0967},{\"city\":\"邢台市\",\"city_code\":\"130500\",\"station\":\"邢师高专\",\"station_code\":\"1078A\",\"lng\":114.5261,\"lat\":37.0533},{\"city\":\"邢台市\",\"city_code\":\"130500\",\"station\":\"路桥公司\",\"station_code\":\"1079A\",\"lng\":114.5331,\"lat\":37.0964},{\"city\":\"邢台市\",\"city_code\":\"130500\",\"station\":\"市环保局\",\"station_code\":\"1080A\",\"lng\":114.4854,\"lat\":37.062},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"游泳馆\",\"station_code\":\"1051A\",\"lng\":115.493,\"lat\":38.8632},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"华电二区\",\"station_code\":\"1052A\",\"lng\":115.5223,\"lat\":38.8957},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"接待中心\",\"station_code\":\"1053A\",\"lng\":115.4713,\"lat\":38.9108},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"地表水厂\",\"station_code\":\"1054A\",\"lng\":115.4612,\"lat\":38.8416},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"胶片厂\",\"station_code\":\"1055A\",\"lng\":115.442,\"lat\":38.8756},{\"city\":\"承德市\",\"city_code\":\"130800\",\"station\":\"铁路\",\"station_code\":\"1062A\",\"lng\":117.9664,\"lat\":40.9161},{\"city\":\"承德市\",\"city_code\":\"130800\",\"station\":\"中国银行\",\"station_code\":\"1063A\",\"lng\":117.9525,\"lat\":40.9843},{\"city\":\"承德市\",\"city_code\":\"130800\",\"station\":\"开发区\",\"station_code\":\"1064A\",\"lng\":117.963,\"lat\":40.9359},{\"city\":\"承德市\",\"city_code\":\"130800\",\"station\":\"文化中心\",\"station_code\":\"1065A\",\"lng\":117.8184,\"lat\":40.9733},{\"city\":\"承德市\",\"city_code\":\"130800\",\"station\":\"离宫\",\"station_code\":\"1066A\",\"lng\":117.9384,\"lat\":41.0112},{\"city\":\"沧州市\",\"city_code\":\"130900\",\"station\":\"沧县城建局\",\"station_code\":\"1071A\",\"lng\":116.8854,\"lat\":38.2991},{\"city\":\"沧州市\",\"city_code\":\"130900\",\"station\":\"电视转播站\",\"station_code\":\"1072A\",\"lng\":116.8584,\"lat\":38.3254}]";
                List<Region> regions = JsonConvert.DeserializeObject<List<Region>>(stations);
                List<MonitorPointPollutan> monitorPoints = GetMonitorPointPollutansby9();
                foreach (Region region in regions)
                {
                    String querys = "pubtime=2019-01-11+14%3A00%3A00&station_code=" + region.station_code;
                    String bodys = "";
                    String url = host + path;
                    HttpWebRequest httpRequest = null;
                    HttpWebResponse httpResponse = null;

                    if (0 < querys.Length)
                    {
                        url = url + "?" + querys;
                    }

                    if (host.Contains("https://"))
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                    }
                    else
                    {
                        httpRequest = (HttpWebRequest)WebRequest.Create(url);
                    }
                    httpRequest.Method = method;
                    httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
                    if (0 < bodys.Length)
                    {
                        byte[] data = Encoding.UTF8.GetBytes(bodys);
                        using (Stream stream = httpRequest.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                    }
                    try
                    {
                        httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        httpResponse = (HttpWebResponse)ex.Response;
                    }

                    Stream st = httpResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                    string me = reader.ReadToEnd();
                    message messagee = JsonConvert.DeserializeObject<message>(me);
                    foreach (MonitorPointPollutan m in monitorPoints.Where(m => m.PointName == messagee.data.station))
                    {
                        HourData hourData = new HourData();
                        hourData.Monitor_PollutionId = m.Id;
                        hourData.MonitorTime = dateTime;
                        switch (m.PollutantId)
                        {
                            case 11: hourData.AVGValue = messagee.data.AQI == null ? 0 : Convert.ToDecimal(messagee.data.AQI); break;
                            case 12: hourData.AVGValue = messagee.data.PM2_5 == null ? 0 : Convert.ToDecimal(messagee.data.PM2_5); break;
                            case 13: hourData.AVGValue = messagee.data.CO == null ? 0 : Convert.ToDecimal(messagee.data.CO); break;
                            case 14: hourData.AVGValue = messagee.data.NO2 == null ? 0 : Convert.ToDecimal(messagee.data.NO2); break;
                            case 15: hourData.AVGValue = messagee.data.O3 == null ? 0 : Convert.ToDecimal(messagee.data.O3); break;
                            case 16: hourData.AVGValue = messagee.data.SO2 == null ? 0 : Convert.ToDecimal(messagee.data.SO2); break;
                            case 17: hourData.AVGValue = messagee.data.PM10 == null ? 0 : Convert.ToDecimal(messagee.data.PM10); break;
                        }
                        hourDatas.Add(hourData);
                    }
                }
                #endregion
                AddHourData(hourDatas);
            }
        }
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        /// <summary>
        /// 获取所有一小时内分钟数据
        /// </summary>
        /// <returns></returns>
        private List<MinuteData> GetMinuteDatas()
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = @"select * from minutedata m join monitorpointpollutan mp on m.monitor_pollutionid=mp.id where m.monitortime> sysdate+numtodsinterval(-1,'hour')";
                var result = conn.Query<MinuteData>(sql, null);
                return result.ToList();
            }
        }
        //进行小时数据的添加
        private void AddHourData(List<HourData> hourDatas)
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = @"insert into hourdata  (avgvalue, monitor_pollutionid, maxvalue, minvalue, monitortime) values ( :AVGValue, :Monitor_PollutionId, :MaxValue, :MinValue, :MonitorTime)";
                conn.Execute(sql, hourDatas);
            }
        }
        #endregion
        #region 日数据
        private void timer3_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == 12)
            {
                //取得一天内小时数据
                List<HourData> hourDatas = GetHourDatas();
                //获取监测点污染物
                List<MonitorPointPollutan> monitorPointPollutans = GetMonitorPointPollutans();
                //用于数据库中一天浓度的添加
                List<DayData> dayDatas = new List<DayData>();
                //用于求一小时的AQI
                List<AvgData> avgDatas = new List<AvgData>();
                DateTime dateTime = DateTime.Now;
                //循环遍历除AQI之外的监测点污染物
                foreach (MonitorPointPollutan monitorPointPollutan in monitorPointPollutans.Where(m => m.PollutantId != 11))
                {
                    DayData dayData = new DayData();
                    AvgData avgData = new AvgData();
                    var mpHourDatas = hourDatas.Where(m => m.Monitor_PollutionId == monitorPointPollutan.Id);

                    dayData.Monitor_PollutionId = monitorPointPollutan.Id;
                    dayData.MaxValue = mpHourDatas.Max(m => m.AVGValue);
                    dayData.MinValue = mpHourDatas.Min(m => m.AVGValue);
                    dayData.AVGValue = mpHourDatas.Average(m => m.AVGValue);
                    dayData.MonitorTime = dateTime;
                    dayDatas.Add(dayData);

                    avgData.AvgValue = mpHourDatas.Average(m => m.AVGValue);
                    avgData.PointID = monitorPointPollutan.PointId;
                    avgData.PollutantID = monitorPointPollutan.PollutantId;
                    avgDatas.Add(avgData);
                }
                AQICompute aqiCompute = new AQICompute();
                foreach (MonitorPointPollutan monitorPointPollutan in monitorPointPollutans.Where(m => m.PollutantId == 11))
                {
                    DayData dayData = new DayData();
                    dayData.Monitor_PollutionId = monitorPointPollutan.Id;
                    dayData.MonitorTime = dateTime;
                    decimal pm2 = 0;
                    decimal pm10 = 0;
                    decimal co = 0;
                    decimal no2 = 0;
                    decimal o3 = 0;
                    decimal so2 = 0;
                    foreach (AvgData avgData in avgDatas)
                    {
                        if (avgData.PointID == monitorPointPollutan.PointId)
                        {
                            switch (avgData.PollutantID)
                            {
                                case 12: pm2 = avgData.AvgValue; break;
                                case 13: co = avgData.AvgValue; break;
                                case 14: no2 = avgData.AvgValue; break;
                                case 15: o3 = avgData.AvgValue; break;
                                case 16: so2 = avgData.AvgValue; break;
                                case 17: pm10 = avgData.AvgValue; break;
                            }
                        }
                    }
                    dayData.AVGValue = Convert.ToDecimal(aqiCompute.GetAQI(pm2, co, no2, o3, so2, pm10));
                    dayDatas.Add(dayData);
                }
                AddDayData(dayDatas);
            }
        }
        /// <summary>
        /// 获取所有一小时内分钟数据
        /// </summary>
        /// <returns></returns>
        private List<HourData> GetHourDatas()
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = @"select * from hourdata m join monitorpointpollutan mp on m.monitor_pollutionid=mp.id where m.monitortime> sysdate+numtodsinterval(-1,'day')";
                var result = conn.Query<HourData>(sql, null);
                return result.ToList();
            }
        }
        //进行小时数据的添加
        private void AddDayData(List<DayData> dayDatas)
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = @"insert into daydata  (avgvalue, monitor_pollutionid, maxvalue, minvalue, monitortime) values ( :AVGValue, :Monitor_PollutionId, :MaxValue, :MinValue, :MonitorTime)";
                conn.Execute(sql, dayDatas);
            }
        }
        #endregion
        #region 月数据
        private void timer4_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Day == 1)
            {
                //取得一月内日数据
                List<DayData> dayDatas = GetDayDatas();
                //获取监测点污染物
                List<MonitorPointPollutan> monitorPointPollutans = GetMonitorPointPollutans();
                //用于数据库中一天浓度的添加
                List<MonthData> monthDatas = new List<MonthData>();
                //用于求月的AQI
                List<AvgData> avgDatas = new List<AvgData>();
                DateTime dateTime = DateTime.Now;
                //循环遍历除AQI之外的监测点污染物
                foreach (MonitorPointPollutan monitorPointPollutan in monitorPointPollutans.Where(m => m.PollutantId != 11))
                {
                    MonthData monthData = new MonthData();
                    AvgData avgData = new AvgData();
                    var mpDayDatas = dayDatas.Where(m => m.Monitor_PollutionId == monitorPointPollutan.Id).ToList();

                    monthData.Monitor_PollutionId = monitorPointPollutan.Id;
                    monthData.MaxValue = mpDayDatas.Max(m => m.AVGValue);
                    monthData.MinValue = mpDayDatas.Min(m => m.AVGValue);
                    if (monitorPointPollutan.PollutantId == 15)
                    {
                        monthData.AVGValue = mpDayDatas[(1 + mpDayDatas.Count() * 90 / 100) - 1].AVGValue;
                    }
                    else if (monitorPointPollutan.PollutantId == 15)
                    {
                        monthData.AVGValue = mpDayDatas[(1 + mpDayDatas.Count() * 95 / 100) - 1].AVGValue;
                    }
                    else
                    {
                        monthData.AVGValue = mpDayDatas.Average(m => m.AVGValue);
                    }
                    monthData.MonitorTime = dateTime;
                    monthDatas.Add(monthData);

                    avgData.AvgValue = mpDayDatas.Average(m => m.AVGValue);
                    avgData.PointID = monitorPointPollutan.PointId;
                    avgData.PollutantID = monitorPointPollutan.PollutantId;
                    avgDatas.Add(avgData);
                }
                AQICompute aqiCompute = new AQICompute();
                foreach (MonitorPointPollutan monitorPointPollutan in monitorPointPollutans.Where(m => m.PollutantId == 11))
                {
                    MonthData monthData = new MonthData();
                    monthData.Monitor_PollutionId = monitorPointPollutan.Id;
                    monthData.MonitorTime = dateTime;
                    decimal pm2 = 0;
                    decimal pm10 = 0;
                    decimal co = 0;
                    decimal no2 = 0;
                    decimal o3 = 0;
                    decimal so2 = 0;
                    foreach (AvgData avgData in avgDatas)
                    {
                        if (avgData.PointID == monitorPointPollutan.PointId)
                        {
                            switch (avgData.PollutantID)
                            {
                                case 12: pm2 = avgData.AvgValue; break;
                                case 13: co = avgData.AvgValue; break;
                                case 14: no2 = avgData.AvgValue; break;
                                case 15: o3 = avgData.AvgValue; break;
                                case 16: so2 = avgData.AvgValue; break;
                                case 17: pm10 = avgData.AvgValue; break;
                            }
                        }
                    }
                    monthData.AVGValue = Convert.ToDecimal(aqiCompute.GetAQI(pm2, co, no2, o3, so2, pm10));
                    monthDatas.Add(monthData);
                }
                AddMonthData(monthDatas);
            }
        }
        /// <summary>
        /// 获取所有一月内日数据
        /// </summary>
        /// <returns></returns>
        private List<DayData> GetDayDatas()
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = @"select * from daydata m join monitorpointpollutan mp on m.monitor_pollutionid=mp.id where m.monitortime> sysdate+numtoyminterval(-1,'month')";
                var result = conn.Query<DayData>(sql, null);
                return result.ToList();
            }
        }
        //进行小时数据的添加
        private void AddMonthData(List<MonthData> monthDatas)
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = @"insert into monthdata  (avgvalue, monitor_pollutionid, maxvalue, minvalue, monitortime) values ( :AVGValue, :Monitor_PollutionId, :MaxValue, :MinValue, :MonitorTime)";
                conn.Execute(sql, monthDatas);
            }
        }
        #endregion
        /// <summary>
        /// 获取所有站点监测污染物关联数据
        /// </summary>
        /// <returns></returns>
        public List<MonitorPointPollutan> GetMonitorPointPollutans()
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = string.Format("select mp.* from monitorpoint m join monitorpointpollutan mp on m.id= mp.pointid where m.pointtype!=9");
                var collectList = conn.Query<MonitorPointPollutan>(sql, null);
                return collectList.ToList();
            }
        }
        /// <summary>
        /// 获取所有站点监测污染物关联数据
        /// </summary>
        /// <returns></returns>
        public List<MonitorPointPollutan> GetMonitorPointPollutansby9()
        {
            using (OracleConnection conn = new OracleConnection("Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan"))
            {
                string sql = string.Format("select mp.*,m.pointname from monitorpoint m join monitorpointpollutan mp on m.id= mp.pointid where m.pointtype=9");
                var collectList = conn.Query<MonitorPointPollutan>(sql, null);
                return collectList.ToList();
            }
        }
        #region api接口类
        //用于接收接口中调取国控站数据的类
        class message
        {
            public bool success { get; set; }
            public QData data { get; set; }
        }
        class QData
        {
            //站名
            public string station { get; set; }

            //各类污染物参数
            public decimal? AQI { get; set; }
            public decimal? SO2 { get; set; }
            public decimal? NO2 { get; set; }
            public decimal? CO { get; set; }
            public decimal? O3 { get; set; }
            public decimal? PM2_5 { get; set; }
            public decimal? PM10 { get; set; }
        }
        #endregion
        //用于接收国控站json文件的类
        public class Region
        {
            public string city { get; set; }
            public string city_code { get; set; }
            public string station { get; set; }
            public string station_code { get; set; }
            public decimal lng { get; set; }
            public decimal lat { get; set; }

        }
    }
}
