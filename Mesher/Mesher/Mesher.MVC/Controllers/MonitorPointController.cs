﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Mesher.Entity;

namespace Mesher.MVC.Controllers
{
    public class MonitorPointController : Controller
    {
        // GET: MonitorPoint
        public ActionResult Index()
        {
            
            return View();
        }

        /// <summary>
        /// 根据行政区名称获取城市code用于调天气预报
        /// </summary>
        /// <param name="cityname"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetCityInfos(string cityname)
        {
            string file = "C:\\Users\\赵新宇\\Source\\Repos\\WebApplication8\\WebApplication8\\_city.json";

            using (System.IO.StreamReader streamreader = System.IO.File.OpenText(file))
            {
                using (JsonTextReader reader = new JsonTextReader(streamreader))
                {
                    List<CityInfo> cityInfos = new JsonSerializer().Deserialize<List<CityInfo>>(reader);
                    foreach (var item in cityInfos)
                    {
                        if (item.city_name == cityname)
                        {
                            return item.city_code;
                        }

                    }
                    return null;

                }
            }
        }
    }

    
}