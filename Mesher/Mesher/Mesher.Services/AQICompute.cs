using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesher.Services
{
    /// <summary>
    /// AQI计算公式
    /// </summary>
    public class AQICompute
    {
        public int GetAllAQI()
        {
            return 1;
        }
        public int GetAQI(decimal pm2,decimal co,decimal no2,decimal o3,decimal so2,decimal pm10)
        {
            //实例化几个计算AQI需要的参数
            //decimal BpHi = 0;//（相对地区的空气质量分指数及对应的污染物项目浓度指数表）中与浓度相近的污染物浓度限值的高位值
            //decimal BpLo = 0;//（相对地区的空气质量分指数及对应的污染物项目浓度指数表）中与浓度相近的污染物浓度限值的高位值
            decimal IAQIHi = 0;//（相对地区的空气质量分指数及对应的污染物项目浓度指数表）中与BPHI对应的空气质量分数
            decimal IAQILo = 0;//（相对地区的空气质量分指数及对应的污染物项目浓度指数表）中与BPLO对应的空气质量分数
            int AQIpm2=0;//pm2.5的AQI
            int AQIco=0;//co的AQI
            int AQIno2=0;//no2的AQI
            int AQIo3=0;//o3的AQI
            int AQIso2=0;//so2的AQI
            int AQIpm10=0;//pm10的AQI
            ///实例化一个污染物类
            AQI a = new AQI();
            a.SO2 = 0;
            a.NO2 = 0;
            a.PM10 = 0;
            a.CO = 0;
            a.O3 = 0;
            a.PM2 = 0;
            a.IAQI = 0;
            AQI a1 = new AQI();
            a1.SO2 = 50;
            a1.NO2 = 40;
            a1.PM10 = 50;
            a1.CO = 2;
            a1.O3 = 100;
            a1.PM2 = 35;
            a1.IAQI = 50;
            AQI a2 = new AQI();
            a2.SO2 = 150;
            a2.NO2 = 80;
            a2.PM10 = 150;
            a2.CO = 4;
            a2.O3 = 160;
            a2.PM2 = 75;
            a2.IAQI = 100;
            AQI a3 = new AQI();
            a3.SO2 = 475;
            a3.NO2 = 180;
            a3.PM10 = 250;
            a3.CO = 14;
            a3.O3 = 215;
            a3.PM2 = 115;
            a3.IAQI = 150;
            AQI a4 = new AQI();
            a4.SO2 = 800;
            a4.NO2 = 280;
            a4.PM10 = 350;
            a4.CO = 240;
            a4.O3 = 265;
            a4.PM2 = 150;
            a4.IAQI = 200;
            AQI a5 = new AQI();
            a5.SO2 = 1600;
            a5.NO2 = 565;
            a5.PM10 = 420;
            a5.CO = 36;
            a5.O3 = 800;
            a5.PM2 = 250;
            a5.IAQI = 300;
            AQI a6 = new AQI();
            a6.SO2 = 2100;
            a6.NO2 = 750;
            a6.PM10 = 500;
            a6.CO = 48;
            a6.O3 = 1000;
            a6.PM2 = 350;
            a6.IAQI = 400;
            AQI a7 = new AQI();
            a7.SO2 = 2620;
            a7.NO2 = 940;
            a7.PM10 = 600;
            a7.CO = 60;
            a7.O3 = 1200;
            a7.PM2 = 500;
            a7.IAQI = 500;
            AQI[] aList = new AQI[]
            {
                a,a1,a2,a3,a4,a5,a6
            };
            for (int i = 0; i < aList.Length - 1; i++)
            {
                ///取出来各个对象的pm2.5的浓度
                if (aList[i].PM2 <= pm2 && pm2 < aList[i + 1].PM2)
                {
                    //最低限度值
                    IAQILo = aList[i].IAQI;
                    //最高限度值
                    IAQIHi= aList[i+1].IAQI;
                    if (pm2 > 500)
                    {
                        AQIpm2 = 500;
                    }
                    else
                    {
                        AQIpm2 = int.Parse(Math.Ceiling((((IAQIHi - IAQILo) / (aList[i + 1].PM2 - aList[i].PM2)) * (pm2 - aList[i].PM2) + IAQILo)).ToString());
                    }
                }

            }
            for (int i = 0; i < aList.Length - 1; i++)
            {
                ///取出来各个对象的pm2.5的浓度
                if (aList[i].CO <= co && co < aList[i + 1].CO)
                {
                    //最低限度值
                    IAQILo = aList[i].IAQI;
                    //最高限度值
                    IAQIHi = aList[i + 1].IAQI;
                    if (co > 60)
                    {
                        AQIco = 500;
                    }
                    else
                    {
                        AQIco = int.Parse(Math.Ceiling((((IAQIHi - IAQILo) / (aList[i + 1].CO - aList[i].CO)) * (co - aList[i].CO) + IAQILo)).ToString());
                    }
                }

            }
            for (int i = 0; i < aList.Length - 1; i++)
            {
                ///取出来各个对象的pm2.5的浓度
                if (aList[i].NO2 <= no2 && no2 < aList[i + 1].NO2)
                {
                    //最低限度值
                    IAQILo = aList[i].IAQI;
                    //最高限度值
                    IAQIHi = aList[i + 1].IAQI;
                    if (no2 > 940)
                    {

                        AQIno2 = 500;
                    }
                    else
                    {
                        AQIno2 = int.Parse(Math.Ceiling((((IAQIHi - IAQILo) / (aList[i + 1].NO2 - aList[i].NO2)) * (no2 - aList[i].NO2) + IAQILo)).ToString());
                    }
                }

            }
            for (int i = 0; i < aList.Length - 1; i++)
            {
                ///取出来各个对象的pm2.5的浓度
                if (aList[i].O3 <= o3 && o3 < aList[i + 1].O3)
                {
                    //最低限度值
                    IAQILo = aList[i].IAQI;
                    //最高限度值
                    IAQIHi = aList[i + 1].IAQI;
                    if (o3 > 1200)
                    {
                        AQIo3 = 500;
                    }
                    else
                    {
                        AQIo3 = int.Parse(Math.Ceiling((((IAQIHi - IAQILo) / (aList[i + 1].O3 - aList[i].O3)) * (o3 - aList[i].O3) + IAQILo)).ToString());
                    }
                }

            }
            for (int i = 0; i < aList.Length - 1; i++)
            {
                ///取出来各个对象的pm2.5的浓度
                if (aList[i].SO2<= so2 && so2 < aList[i + 1].SO2)
                {
                    //最低限度值
                    IAQILo = aList[i].IAQI;
                    //最高限度值
                    IAQIHi = aList[i + 1].IAQI;
                    if (so2 > 2620)
                    {
                        AQIso2 = 500;
                    }
                    else
                    {
                        AQIso2 = int.Parse(Math.Ceiling((((IAQIHi - IAQILo) / (aList[i + 1].SO2 - aList[i].SO2)) * (so2 - aList[i].SO2) + IAQILo)).ToString());
                    } }

            }
            for (int i = 0; i < aList.Length - 1; i++)
            {
                ///取出来各个对象的pm2.5的浓度
                if (aList[i].PM10 <= pm10 && pm10 < aList[i + 1].PM10)
                {
                    //最低限度值
                    IAQILo = aList[i].IAQI;
                    //最高限度值
                    IAQIHi = aList[i + 1].IAQI;
                    if (pm10 > 600)
                    {
                        AQIpm10 = 500;
                    }
                    else
                    {
                        AQIpm10 = int.Parse(Math.Ceiling((((IAQIHi - IAQILo) / (aList[i + 1].PM10 - aList[i].PM10)) * (so2 - aList[i].PM10) + IAQILo)).ToString());

                    }
                }

            }

            int[] arr = { AQIpm2, AQIco,AQIno2,AQIo3,AQIso2,AQIpm10 };
            ArrayList list = new ArrayList(arr);
            list.Sort();
            
            int max = Convert.ToInt32(list[list.Count - 1]);
           
            return max;
            

        }

    }
    /// <summary>
    /// 定义一个污染物的类
    /// </summary>
    public class AQI
    {
        public int SO2 { get; set; }
        public int NO2 { get; set; }
        public int PM10 { get; set; }
        public int CO { get; set; }
        public int O3 { get; set; }
        public int PM2 { get; set; }
        public int IAQI { get; set; }
    }
}
