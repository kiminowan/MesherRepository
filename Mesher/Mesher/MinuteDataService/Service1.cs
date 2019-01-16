using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MinuteDataService
{
    using Mesher.Entity;
    using Mesher.IServices;
    using Mesher.Services;
    using Oracle.ManagedDataAccess.Client;
    using Dapper;

    public partial class Service1 : ServiceBase
    {
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
                AddHourData(hourDatas);
            }
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
    }
}
