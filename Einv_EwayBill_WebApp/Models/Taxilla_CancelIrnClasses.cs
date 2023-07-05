using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    public class Taxilla_CancelIrnClasses
    {
        public static string CancelJsonFile(DataTable dt, string irn, string token,string CanCode)
        {
            string JsTest = "";
            string msg = "",CanRemarks="";
            if (CanCode == "1")
                CanRemarks = "Duplicate Entry";
            else if(CanCode == "2")
                CanRemarks = "Data entry mistake";
            else if (CanCode == "3")
                CanRemarks = "Order Cancelled";
            else if (CanCode == "4")
                CanRemarks = "Others";

            var Calceljson = new CancelRootObject
            {
                irn = dt.Rows[0]["IRN"].ToString(),
                cnlrsn = CanCode,
                cnlrem = CanRemarks
            };

            var objAddjson = Newtonsoft.Json.JsonConvert.SerializeObject(Calceljson);
            return JsTest = objAddjson;
        }
    }

    public class CancelRootObject
    {
        public string irn { get; set; }
        public string cnlrsn { get; set; }
        public string cnlrem { get; set; }
    }

    public class SuccessResult
    {
        public string Irn { get; set; }
        public string CancelDate { get; set; }
    }

    public class EinvCanSuccessRoot
    {
        public bool success { get; set; }
        public string message { get; set; }
        public SuccessResult result { get; set; }
    }
}
