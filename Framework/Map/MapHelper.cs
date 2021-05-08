using Framework.Common.Extension;
using Framework.Map.GaoDe;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace Framework.Map
{
    public class MapHelper
    {
        //地球半径，单位米
        const double EARTH_RADIUS = 6378137;

        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="lat1">第一点纬度</param>
        /// <param name="lng1">第一点经度</param>
        /// <param name="lat2">第二点纬度</param>
        /// <param name="lng2">第二点经度</param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }


        /// <summary>
        /// 将详细的结构化地址转换为高德经纬度坐标
        /// </summary>
        /// <param name="key">API Key（需申请）</param>
        /// <param name="addresses">地址</param>
        /// <returns>返回经纬坐标</returns>
        public static List<string> GetLocationByGaoDe(string key, IList<string> addresses)
        {
            //高德API文档：https://developer.amap.com/api/webservice/guide/api/georegeo

            HttpClient httpClient = new HttpClient();
            string parameter = $"batch=true&key={key}&address={string.Join("|", addresses)}";
            string url = $"https://restapi.amap.com/v3/geocode/geo?{parameter}";
            var response = httpClient.GetAsync(url);
            string responseBody = response.Result.Content.ReadAsStringAsync().Result;
            var responseObj = responseBody.ToObject<LocationOutput>();

            List<string> result = new List<string>();
            responseObj.GeoCodes.ForEach(item => {

                var location = item.Location as string;
                result.Add(location);

            });

            return result;
        }


        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }
    }
}
