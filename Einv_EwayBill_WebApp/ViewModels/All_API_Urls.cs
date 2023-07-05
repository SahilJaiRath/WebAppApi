using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.ViewModels
{
    public class All_API_Urls
    {
        public static string EinvLoginUrl = "https://clientbasic.mastersindia.co/oauth/access_token";
        public static string EinvGenerateUrl = "https://clientbasic.mastersindia.co/generateEinvoice";
        public static string EinvcancelUrl = "https://clientbasic.mastersindia.co/cancelEinvoice";
        public static string EinvGetUrl = "https://clientbasic.mastersindia.co/getEinvoiceData?";
        public static string EwbByIrnUrl = "https://clientbasic.mastersindia.co/generateEwaybillByIrn";
        public static string bulkeinvoice = "https://clientbasic.mastersindia.co/bulkEinvoiceGenerate";


        public static string EWBPROD_Auth_API = "https://pro.mastersindia.co/oauth/access_token";
        public static string Reg_User_MI_API = "https://pro.mastersindia.co/bussiness/checkGstin";

        public static string EWBGenerateUrl = "https://pro.mastersindia.co/ewayBillsGenerate";
        public static string CancelEWBUrl = "https://pro.mastersindia.co/ewayBillCancel";

        public static string Upd_Vno_EWBUrl = "https://clientbasic.mastersindia.co/updateVehicleNumber";
        public static string Consolidated_EWBUrl = "https://clientbasic.mastersindia.co/consolidatedEwayBillsGenerate";
        public static string Reject_EWBUrl = "https://clientbasic.mastersindia.co/ewayBillReject";
        public static string Get_EWBUrl = "https://clientbasic.mastersindia.co/getEwayBillData?";
        public static string Bulk_EWBUrl = "https://clientbasic.mastersindia.co/bulkEwayBillsGenerate";
        public static string Delete_Hold_EWBUrl = "https://clientbasic.mastersindia.co/deleteHoldEwayBill";
        public static string Extend_Validity_EWBUrl = "https://clientbasic.mastersindia.co/ewayBillValidityExtend";
        public static string Update_TransporterId_EWBUrl = "https://clientbasic.mastersindia.co/transporterIdUpdate";
        public static string Calculate_Dist_EWBUrl = "http://clientbasic.mastersindia.co/distance?";





        public static void IgnoreBadCertificates()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }

        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

       
    }
}
