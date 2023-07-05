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
    public class EwaybillByIrn
    {
        public static string Generate_EwaybillIrn_Json(DataTable dt, string sessionId, string access_token)
        {
            string msg = "";
            try
            {
                List<e_waybillIrnBO> ewaybillIrn_obj = new List<e_waybillIrnBO>();
                e_waybillIrnBO rootDtls = new e_waybillIrnBO
                {
                    access_token = access_token,
                    user_gstin = dt.Rows[0]["GSTIN"].ToString(),
                    irn = dt.Rows[0]["IRN"].ToString(),
                    transporter_id = dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString(),
                    transportation_mode = dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString(),
                    transporter_document_number = dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString(),
                    transporter_document_date = Convert.ToDateTime(dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"]).ToString("dd/MM/yyyy"),
                    vehicle_number = dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString(),
                    distance =Convert.ToInt32(dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString()),
                    vehicle_type = dt.Rows[0]["EWAY_TRANSPORTAR_VEHITYPE"].ToString(),
                    transporter_name = dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString(),
                    data_source = "erp"

                };
                ewaybillIrn_obj.Add(rootDtls);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(ewaybillIrn_obj);
                string mystr = json.Substring(1, json.Length - 2);
                return msg = mystr;
            }
            catch (Exception ex)
            {
                msg = "0";
                return msg;
            }
        }


        public static async Task<dynamic> Generate_EwaybillIrn(dynamic JsonFile, string sekdec, DataTable dt, string Type)
        {
            dynamic InvResult = "", InvResDecData = "";
            string InvRData = "";
            HttpResponseMessage response = null;
            try
            {
                string URL = All_API_Urls.EwbByIrnUrl;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(JsonFile, Encoding.UTF8, "application/json");
                    //HttpResponseMessage response = client.PostAsJsonAsync(URL, jsonstring).Result;
                    response = await client.PostAsync(new Uri(URL), content);
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        try
                        {
                            var value2 = JsonConvert.DeserializeObject<SuccessEwbByIrn>(LRData2);
                            SuccessEwbByIrn obj = value2;
                            OracelDataInsert.UpdateEWBByIRNNO(obj, dt);
                            //SqlConnectionDB.InsertDataEwaybillCancel1(obj, dt, Type);

                        }
                        catch (Exception ex)
                        {
                            var value2 = JsonConvert.DeserializeObject<ErrorEwbByIrn>(LRData2);
                            ErrorEwbByIrn obj = value2;
                            string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where id='" + dt.Rows[0]["ID"].ToString() + "' and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                            //SqlConnectionDB.InsertDataEwaybillCancel2(obj, dt, Type);
                        }

                    }
                }
                return InvResult;
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

    public class e_waybillIrnBO
    {
        public string access_token { get; set; }
        public string user_gstin { get; set; }
        public string irn { get; set; }
        public string transporter_id { get; set; }
        public string transportation_mode { get; set; }
        public string transporter_document_number { get; set; }
        public string transporter_document_date { get; set; }
        public string vehicle_number { get; set; }
        public int distance { get; set; }
        public string vehicle_type { get; set; }
        public string transporter_name { get; set; }
        public string data_source { get; set; }
    }

    public class SuccessMessage
    {
        public long EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
    }

    public class SuccessResults
    {
        public SuccessMessage message { get; set; }
        public string errorMessage { get; set; }
        public string InfoDtls { get; set; }
        public string status { get; set; }
        public int code { get; set; }
    }

    public class SuccessEwbByIrn
    {
        public SuccessResults results { get; set; }
    }

    public class ErrorResults
    {
        public string message { get; set; }
        public string errorMessage { get; set; }
        public string InfoDtls { get; set; }
        public string status { get; set; }
        public int code { get; set; }
    }

    public class ErrorEwbByIrn
    {
        public ErrorResults results { get; set; }
    }
}
