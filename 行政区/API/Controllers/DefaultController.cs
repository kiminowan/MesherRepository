using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        public List<MonitorPoint> aa()
        {

            OracleConnection oracleConnection = new OracleConnection(@"Data Source=172.25.53.26/orcl;Persist Security Info=True;User ID=scott;Password=kiminowan");
            #region List集合
            List<Regions> abc = JsonConvert.DeserializeObject<List<Regions>>("[{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"万寿西宫\",\"station_code\":\"1001A\",\"lng\":116.366,\"lat\":39.8673},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"定陵\",\"station_code\":\"1002A\",\"lng\":116.17,\"lat\":40.2865},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"东四\",\"station_code\":\"1003A\",\"lng\":116.434,\"lat\":39.9522},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"天坛\",\"station_code\":\"1004A\",\"lng\":116.434,\"lat\":39.8745},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"农展馆\",\"station_code\":\"1005A\",\"lng\":116.473,\"lat\":39.9716},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"官园\",\"station_code\":\"1006A\",\"lng\":116.361,\"lat\":39.9425},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"海淀区万柳\",\"station_code\":\"1007A\",\"lng\":116.315,\"lat\":39.9934},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"顺义新城\",\"station_code\":\"1008A\",\"lng\":116.72,\"lat\":40.1438},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"怀柔镇\",\"station_code\":\"1009A\",\"lng\":116.644,\"lat\":40.3937},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"昌平镇\",\"station_code\":\"1010A\",\"lng\":116.23,\"lat\":40.1952},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"奥体中心\",\"station_code\":\"1011A\",\"lng\":116.407,\"lat\":40.0031},{\"city\":\"北京市\",\"city_code\":\"110000\",\"station\":\"古城\",\"station_code\":\"1012A\",\"lng\":116.225,\"lat\":39.9279},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"勤俭道\",\"station_code\":\"1015A\",\"lng\":117.145,\"lat\":39.1654},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"大直沽八号路\",\"station_code\":\"1017A\",\"lng\":117.237,\"lat\":39.1082},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"前进道\",\"station_code\":\"1018A\",\"lng\":117.202,\"lat\":39.0927},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"淮河道\",\"station_code\":\"1019A\",\"lng\":117.1837,\"lat\":39.2133},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"跃进路\",\"station_code\":\"1021A\",\"lng\":117.307,\"lat\":39.0877},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"第四大街\",\"station_code\":\"1023A\",\"lng\":117.707,\"lat\":39.0343},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"永明路\",\"station_code\":\"1024A\",\"lng\":117.457,\"lat\":38.8394},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"汉北路\",\"station_code\":\"1026A\",\"lng\":117.764,\"lat\":39.1587},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"团泊洼\",\"station_code\":\"1027A\",\"lng\":117.157,\"lat\":38.9194},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"河西一经路\",\"station_code\":\"2858A\",\"lng\":117.7918,\"lat\":39.2474},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"津沽路\",\"station_code\":\"2859A\",\"lng\":117.3747,\"lat\":38.9846},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"宾水西道\",\"station_code\":\"2860A\",\"lng\":117.1589,\"lat\":39.0845},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"大理道\",\"station_code\":\"2922A\",\"lng\":117.1941,\"lat\":39.1067},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"中山北路\",\"station_code\":\"3020A\",\"lng\":117.2099,\"lat\":39.16969},{\"city\":\"天津市\",\"city_code\":\"120000\",\"station\":\"西四道\",\"station_code\":\"3051A\",\"lng\":117.3916,\"lat\":39.1495},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"职工医院\",\"station_code\":\"1029A\",\"lng\":114.4548,\"lat\":38.0513},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"高新区\",\"station_code\":\"1030A\",\"lng\":114.6046,\"lat\":38.0398},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"西北水源\",\"station_code\":\"1031A\",\"lng\":114.5019,\"lat\":38.1398},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"西南高教\",\"station_code\":\"1032A\",\"lng\":114.4586111,\"lat\":38.00583333},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"世纪公园\",\"station_code\":\"1033A\",\"lng\":114.5330556,\"lat\":38.01777778},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"人民会堂\",\"station_code\":\"1034A\",\"lng\":114.5214,\"lat\":38.0524},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"封龙山\",\"station_code\":\"1035A\",\"lng\":114.3541,\"lat\":37.9097},{\"city\":\"石家庄市\",\"city_code\":\"130100\",\"station\":\"22中南校区\",\"station_code\":\"2862A\",\"lng\":114.5480556,\"lat\":38.03777778},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"供销社\",\"station_code\":\"1036A\",\"lng\":118.1662,\"lat\":39.6308},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"雷达站\",\"station_code\":\"1037A\",\"lng\":118.144,\"lat\":39.643},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"物资局\",\"station_code\":\"1038A\",\"lng\":118.1853,\"lat\":39.6407},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"陶瓷公司\",\"station_code\":\"1039A\",\"lng\":118.2185,\"lat\":39.6679},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"十二中\",\"station_code\":\"1040A\",\"lng\":118.1838,\"lat\":39.65782},{\"city\":\"唐山市\",\"city_code\":\"130200\",\"station\":\"小山\",\"station_code\":\"1041A\",\"lng\":118.1997,\"lat\":39.6295},{\"city\":\"秦皇岛市\",\"city_code\":\"130300\",\"station\":\"北戴河环保局\",\"station_code\":\"1042A\",\"lng\":119.5259,\"lat\":39.8283},{\"city\":\"秦皇岛市\",\"city_code\":\"130300\",\"station\":\"第一关\",\"station_code\":\"1043A\",\"lng\":119.7624,\"lat\":40.0181},{\"city\":\"秦皇岛市\",\"city_code\":\"130300\",\"station\":\"监测站\",\"station_code\":\"1044A\",\"lng\":119.6023,\"lat\":39.9567},{\"city\":\"秦皇岛市\",\"city_code\":\"130300\",\"station\":\"建设大厦\",\"station_code\":\"1046A\",\"lng\":119.5369,\"lat\":39.9419},{\"city\":\"秦皇岛市\",\"city_code\":\"130300\",\"station\":\"文明里(启用171012)\",\"station_code\":\"3132A\",\"lng\":119.5967,\"lat\":37.0308},{\"city\":\"邯郸市\",\"city_code\":\"130400\",\"station\":\"环保局\",\"station_code\":\"1047A\",\"lng\":114.5129,\"lat\":36.61763},{\"city\":\"邯郸市\",\"city_code\":\"130400\",\"station\":\"东污水处理厂\",\"station_code\":\"1048A\",\"lng\":114.5426,\"lat\":36.6164},{\"city\":\"邯郸市\",\"city_code\":\"130400\",\"station\":\"矿院\",\"station_code\":\"1049A\",\"lng\":114.5035,\"lat\":36.5776},{\"city\":\"邯郸市\",\"city_code\":\"130400\",\"station\":\"丛台公园\",\"station_code\":\"1050A\",\"lng\":114.4965,\"lat\":36.61981},{\"city\":\"邢台市\",\"city_code\":\"130500\",\"station\":\"达活泉\",\"station_code\":\"1077A\",\"lng\":114.4821,\"lat\":37.0967},{\"city\":\"邢台市\",\"city_code\":\"130500\",\"station\":\"邢师高专\",\"station_code\":\"1078A\",\"lng\":114.5261,\"lat\":37.0533},{\"city\":\"邢台市\",\"city_code\":\"130500\",\"station\":\"路桥公司\",\"station_code\":\"1079A\",\"lng\":114.5331,\"lat\":37.0964},{\"city\":\"邢台市\",\"city_code\":\"130500\",\"station\":\"市环保局\",\"station_code\":\"1080A\",\"lng\":114.4854,\"lat\":37.062},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"游泳馆\",\"station_code\":\"1051A\",\"lng\":115.493,\"lat\":38.8632},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"华电二区\",\"station_code\":\"1052A\",\"lng\":115.5223,\"lat\":38.8957},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"接待中心\",\"station_code\":\"1053A\",\"lng\":115.4713,\"lat\":38.9108},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"地表水厂\",\"station_code\":\"1054A\",\"lng\":115.4612,\"lat\":38.8416},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"胶片厂\",\"station_code\":\"1055A\",\"lng\":115.442,\"lat\":38.8756},{\"city\":\"保定市\",\"city_code\":\"130600\",\"station\":\"监测站\",\"station_code\":\"1056A\",\"lng\":115.5214,\"lat\":38.8707},{\"city\":\"承德市\",\"city_code\":\"130800\",\"station\":\"铁路\",\"station_code\":\"1062A\",\"lng\":117.9664,\"lat\":40.9161},{\"city\":\"承德市\",\"city_code\":\"130800\",\"station\":\"中国银行\",\"station_code\":\"1063A\",\"lng\":117.9525,\"lat\":40.9843},{\"city\":\"承德市\",\"city_code\":\"130800\",\"station\":\"开发区\",\"station_code\":\"1064A\",\"lng\":117.963,\"lat\":40.9359},{\"city\":\"承德市\",\"city_code\":\"130800\",\"station\":\"文化中心\",\"station_code\":\"1065A\",\"lng\":117.8184,\"lat\":40.9733},{\"city\":\"承德市\",\"city_code\":\"130800\",\"station\":\"离宫\",\"station_code\":\"1066A\",\"lng\":117.9384,\"lat\":41.0112},{\"city\":\"沧州市\",\"city_code\":\"130900\",\"station\":\"沧县城建局\",\"station_code\":\"1071A\",\"lng\":116.8854,\"lat\":38.2991},{\"city\":\"沧州市\",\"city_code\":\"130900\",\"station\":\"电视转播站\",\"station_code\":\"1072A\",\"lng\":116.8584,\"lat\":38.3254},{\"city\":\"沧州市\",\"city_code\":\"130900\",\"station\":\"市环保局\",\"station_code\":\"1073A\",\"lng\":116.8709,\"lat\":38.3228}]");
            #endregion

            //List<Region> regions = new List<Region>();
            //foreach (var item in abc)
            //{
            //    Region region = new Region();
            //    region.RegionName = item.city;
            //    region.Region_code = item.city_code;
            //    if (!regions.Exists(m => m.RegionName.Equals(region.RegionName)))
            //    {
            //        regions.Add(region);

            //    }
            //}
            //string executeSql = @" INSERT INTO Region (REGIONNAME,REGION_CODE) VALUES (:RegionName,:Region_code) ";
            //oracleConnection.Execute(executeSql, regions);
            //return regions;



            List<MonitorPoint> monitorPoints = new List<MonitorPoint>();
            foreach (var item in abc)
            {
                MonitorPoint monitorPoint = new MonitorPoint();
                monitorPoint.PointName = item.station;
                monitorPoint.Longitude = item.lng;
                monitorPoint.Latitude = item.lat;
                monitorPoint.PointType = 1;
                monitorPoint.RegionCode = item.city_code;
                monitorPoint.Status = 1;
                if (!monitorPoints.Exists(m=>m.PointName.Equals(monitorPoint.PointName)))
                {
                    monitorPoints.Add(monitorPoint);
                }
            }
            //名称、经、纬、设备编号、类别、行政区编号、状态、时间、最近国控点
            string exSql = @" INSERT INTO MonitorPoint (POINTNAME,LONGITUDE,LATITUDE,EQUIPMENTCODE,POINTTYPE,REGIONCODE,STATUS,UPDATEDATE) VALUES (:PointName,:Longitude,:Latitude,sys_guid(),:PointType,:RegionCode,:Status,sysdate) ";
            oracleConnection.Execute(exSql, monitorPoints);
            return monitorPoints;



            //int Add = oracleConnection.Execute("insert into Region(REGIONNAME,REGION_CODE) values ('1','2')");
            //"\"" + regions.City + "\",\"" + regions.City_code + "\")
        }

    }
}
