using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.ViewModels
{
    public class GetAccessToken
    {
        public static async Task<dynamic> Einvoice_API_Login(DataTable dt)
        {
            dynamic InvResult;
            try
            {

                //string URL = "https://clientbasic.mastersindia.co/oauth/access_token";
                string URL = All_API_Urls.EinvLoginUrl;
                var jsonstring = GetJsonAuthentication_API(dt);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;                  
                    // ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;                   
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    //HttpResponseMessage response = await client.PostAsync(new Uri(URL), content);

                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        dynamic json = JValue.Parse(InvResult);
                        if (json.Status == 0)
                        {
                            string ErrorMsg = "Error";
                        }
                        else
                        {
                            string LRData2 = response.Content.ReadAsStringAsync().Result;
                            var value2 = JsonConvert.DeserializeObject<Authentication_API_Response>(LRData2);
                            InvResult = value2.access_token;
                        }
                    }
                    else
                    {

                        InvResult = response.Content.ReadAsStringAsync().Result;
                        //ErrorLog.WriteLog(InvResult);
                        InvResult = "";
                    }
                }
                return InvResult;
            }
            catch (AggregateException err)
            {

                return InvResult = err.Message;
            }
        }

        public static string GetJsonAuthentication_API(DataTable dt)
        {
            Authentication_API Authentication_APIobj = new Authentication_API
            {
                username = dt.Rows[0]["EINVUSERNAME"].ToString(),
                password = dt.Rows[0]["EINVPASSWORD"].ToString(),
                client_id = dt.Rows[0]["EFUSERNAME"].ToString(),
                client_secret = dt.Rows[0]["EFPASSWORD"].ToString(),
                grant_type = "password",
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Authentication_APIobj);
            return json.ToString();
        }

    }

    public class Authentication_API
    {
        public string username { get; set; }
        public string password { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
    }

    public class Authentication_API_Response
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
    }
}
