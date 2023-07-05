using Einv_EwayBill_WebApp.Models;
using Newtonsoft.Json;
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
    public class CanEinvoice
    {
        public static string CancelJsonFile(DataTable dt, string irn, string token)
        {
            string JsTest = "";
            string msg = "";
            try
            {
                var Calceljson = new CancelRootObject
                {
                    access_token = token,
                    user_gstin = dt.Rows[0]["GSTIN"].ToString(),
                    irn = dt.Rows[0]["IRN"].ToString(),
                    cancel_reason = "1 ",
                    cancel_remarks = "cancel Remarks"
                };

                var objAddjson = Newtonsoft.Json.JsonConvert.SerializeObject(Calceljson);
                return JsTest = objAddjson;
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteLog(ex);
            }
            return JsTest;

        }


        public static async Task<string> CancelInvoice(string JsonFile, string sekdec, DataTable dt)
        {
            string InvRData = "";
            dynamic InvResult = "", InvResDecData = "";
            string ErrorMsg = "";
            try
            {
                // string URL = "https://clientbasic.mastersindia.co/cancelEinvoice";
                string URL = All_API_Urls.EinvcancelUrl;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(JsonFile, Encoding.UTF8, "application/json");
                    // ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    //HttpResponseMessage response = client.PostAsJsonAsync(URL, jsonstring).Result;
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        var value2 = JsonConvert.DeserializeObject<CancelRoot>(LRData2);
                        CancelRoot obj = value2;

                        //SqlConnectionDB.CancelInesrtData(obj, dt);
                        OracelDataInsert.InsertCancelDataOracle(obj, dt);
                        if (value2.results.errorMessage != "")
                        {
                            //InsertCancelDataOracle
                            //error update 
                        }
                        else
                        {
                            //Irn number and cancel date update
                        }


                    }
                }
                return InvRData = InvResult;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where id='" + dt.Rows[0]["ID"].ToString() + "' and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                return InvRData = ex.Message;
            }

        }
    }

    public class CancelRootObject
    {
        public string access_token { get; set; }
        public string user_gstin { get; set; }
        public string irn { get; set; }
        public string cancel_reason { get; set; }
        public string cancel_remarks { get; set; }
    }

    public class CncelMessage
    {
        public string Irn { get; set; }
        public string CancelDate { get; set; }
    }
    public class CancelResults
    {
        public CncelMessage message { get; set; }
        public string errorMessage { get; set; }
        public string InfoDtls { get; set; }
        public string status { get; set; }
        public int code { get; set; }
    }

    public class CancelRoot
    {
        public CancelResults results { get; set; }
    }
}
