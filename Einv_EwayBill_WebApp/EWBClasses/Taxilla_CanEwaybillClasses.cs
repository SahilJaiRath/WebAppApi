using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.EWBClasses
{
    public class Taxilla_CanEwaybillClasses
    {
        public static string GenerateCancelJson(DataTable dt, string access_token)
        {
            string msg = "";
            List<CanEwayObject> ewaobj = new List<CanEwayObject>();
            CanEwayObject rootDtls = new CanEwayObject
            {
                ewbNo = Convert.ToInt64(dt.Rows[0]["EWBNO"].ToString()),
                cancelRsnCode = 1,
                cancelRmrk = "Others",
            };
            ewaobj.Add(rootDtls);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ewaobj);
            string mystr = json.Substring(1, json.Length - 2);
            return msg = mystr;
        }
    }

    public class CanEwayObject
    {
        public long ewbNo { get; set; }
        public int cancelRsnCode { get; set; }
        public string cancelRmrk { get; set; }
    }

    public class CanEwaySuccessResult
    {
        public string cancelDate { get; set; }
        public string ewayBillNo { get; set; }
    }

    public class CanEwaybillSuccessRoot
    {
        public bool success { get; set; }
        public CanEwaySuccessResult result { get; set; }
        public string message { get; set; }
    }
}
