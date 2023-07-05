using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    public class DistanceResponseClass
    {
        public static string GoogleDistAPI(int frompin, int topin)
        {
            string InvRData = "";
            dynamic InvResult = "";
            int dist = 0;
            try
            {
                string URL = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + frompin + "&destinations=" + topin + "&key=AIzaSyBi_rPFuYXG-77gPrtzAh-IHqqLmrlrrAs";
                ClsDynamic.WriteLog(URL);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    HttpResponseMessage response = client.GetAsync(URL, HttpCompletionOption.ResponseContentRead).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        //string LRData2 = await response.Content.ReadAsStringAsync();
                        var value2 = JsonConvert.DeserializeObject<Root>(InvResult);
                        if (value2.status == "OK")
                        {
                            InvRData = value2.rows[0].elements[0].distance.value.ToString();
                            dist = Convert.ToInt32(Convert.ToDecimal(InvRData) / 1000);
                            //dist = Convert.ToInt32(Convert.ToDecimal(dist - ((dist / 100) * 15)));                        
                        }
                    }
                }
                return InvRData = dist.ToString();
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog(ex.Message);
                return InvRData = "0";
            }

        }


        public static string EwaybillDistance(int frompin, int topin, string AccessToken)
        {
            string InvRData = "";
            dynamic InvResult = "";
            int dist = 0;
            try
            {

                string URL = "http://clientbasic.mastersindia.co/distance?access_token=" + AccessToken + "&fromPincode=" + frompin + "&toPincode=" + topin + "";
                //ClsDynamic.WriteLog(URL);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    HttpResponseMessage response = client.GetAsync(URL, HttpCompletionOption.ResponseContentRead).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        //string LRData2 = await response.Content.ReadAsStringAsync();
                        var DistData = JsonConvert.DeserializeObject<MIDistanceRoot>(InvResult);
                        MIDistanceRoot obj = DistData;
                        if (obj.results.status == "Success")
                        {
                            InvRData = obj.results.distance.ToString();
                            //dist = Convert.ToInt32(Convert.ToDecimal(InvRData) / 1000) - 5;
                            //dist = Convert.ToInt32(Convert.ToDecimal(dist - ((dist / 100) * 15)));                        
                        }
                    }
                }
                return InvRData; ;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog(ex.Message);
                return InvRData = "";
            }

        }
    }

    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }

    public class Row
    {
        public List<Element> elements { get; set; }
    }

    public class Root
    {
        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<Row> rows { get; set; }
        public string status { get; set; }
    }


    public class MIDistanceResults
    {
        public int distance { get; set; }
        public string status { get; set; }
        public int code { get; set; }
    }

    public class MIDistanceRoot
    {
        public MIDistanceResults results { get; set; }
    }
}
