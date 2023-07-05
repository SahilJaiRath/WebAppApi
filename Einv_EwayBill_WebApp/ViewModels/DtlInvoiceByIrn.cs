using Einv_EwayBill_WebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.ViewModels
{
    public class DtlInvoiceByIrn
    {
        public static async Task<string> DetailInvoiceByIrn(string IRN_No, string access_token, DataTable dt)
        {
            string InvRData = "";
            dynamic InvResult = "";
            try
            {
                string URL = All_API_Urls.EinvGetUrl;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    string param = "access_token=" + access_token + "&gstin=" + dt.Rows[0]["GSTIN"].ToString() + "&irn=" + IRN_No;
                    HttpResponseMessage response = client.GetAsync(URL + param, HttpCompletionOption.ResponseContentRead).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        var value2 = JsonConvert.DeserializeObject<INVDtlRootObject>(LRData2);
                        INVDtlRootObject obj = value2;

                        //OracelDataInsert.InsertCancelDataOracle(obj, dt);
                        //SqlConnectionDB.CancelInesrtData(obj, dt);

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

    public class INVDtlMessage
    {
        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }
        public string Status { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public int EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
    }

    public class INVDtlResults
    {
        public INVDtlMessage message { get; set; }
        public string errorMessage { get; set; }

        public string InfoDtls { get; set; }
        public string status { get; set; }
        public int code { get; set; }
    }

    public class INVDtlRootObject
    {
        public INVDtlResults results { get; set; }
    }
}
