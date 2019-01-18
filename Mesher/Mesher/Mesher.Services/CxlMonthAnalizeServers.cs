using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Services
{
    using Mesher.IServices;
    using Mesher.Entity;
    using Dapper;
    using Oracle.ManagedDataAccess.Client;

    /// <summary>
    /// 月度对比分析
    /// </summary>
    public class CxlMonthAnalizeServers : ICxlMonthAnalizeServers
    {
        /// <summary>
        /// 返回月度数据（全部）
        /// </summary>
        /// <returns></returns>
        public List<MonthAnalizeData> GetMonthAnalizeDatas(int Code, DateTime time)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                //传回的是用户id
                string sql1 = string.Format("select a.region_code from Region a,\"User\" b where a.id=b.RegionId and b.id='" + Code + "'");
                string collectList1 = conn.Query<string>(sql1, null).FirstOrDefault();
                string code1 = collectList1;
                //获取到行政区的编号
                string code2 = code1;
                //三表联查
                //string sql = string.Format("select a.avgvalue,a.monitortime,c.PollutantName,d.PointName from MONTHDATA a ,MonitorPointPollutan b, Pollutant c,MonitorPoint d where a.Monitor_PollutionId=b.Id and b.PollutantId=c.id and b.pointid=d.id and d.RegionCode='" + code2 + "'");
                //var collectList = conn.Query<MonthAnalizeData>(sql, null);

                //List<MonthAnalizeData> list= collectList.ToList<MonthAnalizeData>();
                //List<MonthAnalizeData> list2= list.Where(n => n.PointName == "PM2.5").ToList();
                //List<MonthAnalizeData> list3 = list.Where(n => n.PointName == "PM10").ToList();
                //List<MonthAnalizeData> list4 = list.Where(n => n.PointName == "O3").ToList();
                //List<MonthAnalizeData> list5 = list.Where(n => n.PointName == "NO2").ToList();
                //List<MonthAnalizeData> list6 = list.Where(n => n.PointName == "CO").ToList();
                //List<MonthAnalizeData> list7 = list.Where(n => n.PointName == "SO2").ToList();
                //4表连查
                string sql = string.Format("select a.avgvalue,c.pointname,d.pollutantname from MonthData a,MonitorPointPollutan b,MonitorPoint c,Pollutant d where a.Monitor_Pollutionid=b.id and b.PointId=c.id and b.pollutantid=d.id and c.RegionCode='" + code2 + "' and a.monitortime=:time  ");
                //接收
                var collectList = conn.Query<MonthAnalizeData>(sql, new { time });
                //求上一年同月份，用于同比
                DateTime time3 = time.AddYears(-1);
                //求上个月份，用于环比
                DateTime time2 = time.AddMonths(-1);
                //获取同比数据的sql语句
                string sql12 = string.Format("select a.avgvalue, c.pointname, d.pollutantname from MonthData a, MonitorPointPollutan b, MonitorPoint c, Pollutant d where a.Monitor_Pollutionid = b.id and b.PointId = c.id and b.pollutantid = d.id and c.RegionCode = '" + code2 + "' and a.monitortime =:times");
                //用于同比的数据
                var collectList2 = conn.Query<MonthAnalizeData>(sql12, new { times = time3 });
                //获取环比数据的sql语句
                string sql13 = string.Format("select a.avgvalue, c.pointname, d.pollutantname from MonthData a, MonitorPointPollutan b, MonitorPoint c, Pollutant d where a.Monitor_Pollutionid = b.id and b.PointId = c.id and b.pollutantid = d.id and c.RegionCode = '" + code2 + "' and a.monitortime =:timess");
                //获取环比的数据
                var collectList3 = conn.Query<MonthAnalizeData>(sql13, new { timess = time2 });
                //数组接收所有的站点的名称
                List<string> pointNames = (from c in collectList select c.PointName).Distinct().OrderBy(m => m).ToList();
                //实例化一个数组用于返回
                List<MonthAnalizeData> list = new List<MonthAnalizeData>();
                //循环遍历 取出各个站点名称
                foreach (string pointName in pointNames)
                {
                    //实例化对象，用于存储数据
                    MonthAnalizeData monthAnalizeData = new MonthAnalizeData();
                    //赋值
                    monthAnalizeData.PointName = pointName;
                    //循环遍历同一站点的数据
                    foreach (MonthAnalizeData monthAnalizeData1 in collectList.Where(m => m.PointName == pointName))
                    {
                        //同比，因为上一年的数据会没有，所以要有一个非空判断
                        if (collectList2.Count() > 0)
                        {
                            //遍历同一站点的同比数据
                            foreach (MonthAnalizeData monthAnalizeData2 in collectList2.Where(m => m.PointName == pointName))
                            {
                                //遍历同一站点的环比数据
                                foreach (MonthAnalizeData monthAnalizeData3 in collectList3.Where(m => m.PointName == pointName))
                                {
                                    ///同一站点 污染物 的数据
                                    if (monthAnalizeData1.PollutantName == "PM2.5" && monthAnalizeData2.PollutantName == "PM2.5" && monthAnalizeData3.PollutantName == "PM2.5")
                                    {
                                        //赋值
                                        monthAnalizeData.PM2AVGValue = monthAnalizeData1.AVGValue;
                                        //同比赋值
                                        monthAnalizeData.TPM2AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData2.AVGValue;
                                        //环比复制
                                        monthAnalizeData.HPM2AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                    }
                                    ///同一站点 污染物 的数据
                                    if (monthAnalizeData1.PollutantName == "PM10" && monthAnalizeData2.PollutantName == "PM10" && monthAnalizeData3.PollutantName == "PM10")
                                    {
                                        monthAnalizeData.PM10AVGValue = monthAnalizeData1.AVGValue;
                                        monthAnalizeData.TPM10AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData2.AVGValue;
                                        monthAnalizeData.HPM10AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                    }
                                    if (monthAnalizeData1.PollutantName == "CO" && monthAnalizeData2.PollutantName == "CO" && monthAnalizeData3.PollutantName == "CO")
                                    {
                                        monthAnalizeData.COAVGValue = monthAnalizeData1.AVGValue;
                                        monthAnalizeData.TCOAVGValue = monthAnalizeData1.AVGValue / monthAnalizeData2.AVGValue;
                                        monthAnalizeData.HCOAVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                    }
                                    if (monthAnalizeData1.PollutantName == "NO" && monthAnalizeData2.PollutantName == "NO" && monthAnalizeData3.PollutantName == "NO")
                                    {
                                        monthAnalizeData.N02AVGValue = monthAnalizeData1.AVGValue;
                                        monthAnalizeData.TN02AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData2.AVGValue;
                                        monthAnalizeData.HN02AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                    }
                                    if (monthAnalizeData1.PollutantName == "O3" && monthAnalizeData2.PollutantName == "O3" && monthAnalizeData3.PollutantName == "O3")
                                    {
                                        monthAnalizeData.O3AVGValue = monthAnalizeData1.AVGValue;
                                        monthAnalizeData.TO3AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData2.AVGValue;
                                        monthAnalizeData.HO3AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                    }
                                    if (monthAnalizeData1.PollutantName == "SO2" && monthAnalizeData2.PollutantName == "SO2" && monthAnalizeData3.PollutantName == "SO2")
                                    {
                                        monthAnalizeData.SO2AVGValue = monthAnalizeData1.AVGValue;
                                        monthAnalizeData.TSO2AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData2.AVGValue;
                                        monthAnalizeData.HSO2AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                    }
                                }
                            }

                        }
                        ///判断环比数据是否为空
                        else if (collectList3.Count() > 0)
                        {
                            foreach (MonthAnalizeData monthAnalizeData3 in collectList3.Where(m => m.PointName == pointName))
                            {
                                if (monthAnalizeData1.PollutantName == "PM2.5")
                                {
                                    monthAnalizeData.PM2AVGValue = monthAnalizeData1.AVGValue;

                                    monthAnalizeData.HPM2AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                }
                                if (monthAnalizeData1.PollutantName == "PM10")
                                {
                                    monthAnalizeData.PM10AVGValue = monthAnalizeData1.AVGValue;

                                    monthAnalizeData.HPM10AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                }
                                if (monthAnalizeData1.PollutantName == "CO")
                                {
                                    monthAnalizeData.COAVGValue = monthAnalizeData1.AVGValue;

                                    monthAnalizeData.HCOAVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                }
                                if (monthAnalizeData1.PollutantName == "NO")
                                {
                                    monthAnalizeData.N02AVGValue = monthAnalizeData1.AVGValue;

                                    monthAnalizeData.HN02AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                }
                                if (monthAnalizeData1.PollutantName == "O3")
                                {
                                    monthAnalizeData.O3AVGValue = monthAnalizeData1.AVGValue;

                                    monthAnalizeData.HO3AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                }
                                if (monthAnalizeData1.PollutantName == "SO2")
                                {
                                    monthAnalizeData.SO2AVGValue = monthAnalizeData1.AVGValue;

                                    monthAnalizeData.HSO2AVGValue = monthAnalizeData1.AVGValue / monthAnalizeData3.AVGValue;
                                }
                            }
                        }
                        else
                        {
                            if (monthAnalizeData1.PollutantName == "PM2.5")
                                monthAnalizeData.PM2AVGValue = monthAnalizeData1.AVGValue;

                            if (monthAnalizeData1.PollutantName == "PM10")
                                monthAnalizeData.PM10AVGValue = monthAnalizeData1.AVGValue;

                            if (monthAnalizeData1.PollutantName == "CO")
                                monthAnalizeData.COAVGValue = monthAnalizeData1.AVGValue;

                            if (monthAnalizeData1.PollutantName == "NO")
                                monthAnalizeData.N02AVGValue = monthAnalizeData1.AVGValue;

                            if (monthAnalizeData1.PollutantName == "O3")
                                monthAnalizeData.O3AVGValue = monthAnalizeData1.AVGValue;

                            if (monthAnalizeData1.PollutantName == "SO2")
                                monthAnalizeData.SO2AVGValue = monthAnalizeData1.AVGValue;

                        }
                    }
                    //给list赋值
                    list.Add(monthAnalizeData);
                }
                return list;
            }
        }






        /// <summary>
        /// 大气污染物分析
        /// </summary>
        /// <param name="Code">当前国控点的id</param>
        /// <param name="name">距离最近的微站点的名称</param>
        /// <param name="pollname">污染物的名称</param>
        /// <returns></returns>
        public List<NationalControl> GetNationalControls(int Code, string pollname,string name)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                //获取到当前时间
                string time1 = DateTime.Now.ToString();
                string tim2 = DateTime.Now.AddHours(-24).ToString();



                //获取到大气污染物的数据    获取到 离国控点最近的微站点 pm2.5的数据
                string sql = string.Format("select a.avgvalue,a.MonitorTime,c.pointname,d.pollutantname from hourdata a,MonitorPointPollutan b,MonitorPoint c,Pollutant d where a.monitor_pollutionid=b.id and b.pointid=c.id and b.pollutantid=d.id and c.NearlyStation='" + Code + "' and d.pollutantname='" + pollname + "' AND a.monitortime<to_date('" + time1 + "','yyyy-MM-dd hh24:mi:ss') and  a.monitortime>to_date('" + tim2 + "','yyyy-MM-dd hh24:mi:ss') and c.pointname='"+name+"'");
                //接收
                var collectList = conn.Query<NationalControl>(sql, null);
                //按照站名进行排序，并获得所有的站名
                List<string> pointNames = (from c in collectList select c.PointName).Distinct().OrderBy(m => m).ToList();
                List<NationalControl> list = new List<NationalControl>();
                //string sql1 = string.Format("select a.avgvalue,a.MonitorTime,c.pointname,c.id,d.pollutantname from hourdata a, MonitorPointPollutan b,MonitorPoint c, Pollutant d where a.monitor_pollutionid = b.id and b.pointid = c.id and b.pollutantid = d.id and c.NearlyStation = 180 AND a.monitortime < to_date('" + time1 + "', 'yyyy-MM-dd hh24:mi:ss') and a.monitortime > to_date('" + tim2 + "', 'yyyy-MM-dd hh24:mi:ss')");
                // var collectList1 = conn.Query<NationalControl>(sql1, null);
                //循环遍历出所有的站名
                foreach (string pointName in pointNames)
                {
                    //实例化一个站点对象

                    //通过循环取出 距离最近的站点的名字
                    foreach (NationalControl n in collectList.Where(n => n.PointName == pointName))
                    {
                        NationalControl na = new NationalControl();

                        na.PointName = pointName;
                        na.MonitorTime = n.MonitorTime;
                        na.AVGValue = n.AVGValue;
                        list.Add(na);
                    }



                }
                return list;

            }
        }

        /// <summary>
        /// 获取当前行政区的国控点
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public List<MonitorPoint> GetMonitorPoints(int Code)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                //传回的是用户id
                string sql1 = string.Format("select a.region_code from Region a,\"User\" b where a.id=b.RegionId and b.id='" + Code + "'");
                string collectList1 = conn.Query<string>(sql1, null).FirstOrDefault();
                string code1 = collectList1;
                string sql = string.Format("select * from MonitorPoint where regioncode='" + code1 + "' and pointtype=9");
                var collectList = conn.Query<MonitorPoint>(sql, null);
                return collectList.ToList();
            }
        }
        /// <summary>
        /// 根据国控点的id获取距离最近的微站点
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public List<NationalControl> GetZh(int cor)
        {
            using (OracleConnection conn = DapperHelper.GetConnString())
            {
                string sql = string.Format("select * from MonitorPoint where NearlyStation='"+cor+"'");
                var collectList = conn.Query<NationalControl>(sql, null);
                return collectList.ToList();
            }
        }
    }
}
