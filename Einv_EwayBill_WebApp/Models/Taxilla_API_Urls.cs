using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    public class Taxilla_API_Urls
    {
        //public static string AuthApi = "https://gsp.adaequare.com/gsp/authenticate?action=GSP&grant_type=token";
        //public static string GenIrnApi = "https://gsp.adaequare.com/test/enriched/ei/api/invoice";
        //public static string CanIrnApi = "https://gsp.adaequare.com/test/enriched/ei/api/invoice/cancel";
        //public static string GetInvbyIrn = "https://gsp.adaequare.com/test/enriched/ei/api/invoice/irn?";
        //public static string Taxpayerdtl = "https://gsp.adaequare.com/test/enriched/ei/api/master/gstin?gstin=36GSPOHM281G1ZP";
        //public static string GenEwbbyIrn = "https://gsp.adaequare.com/test/enriched/ei/api/ewaybill";
        //public static string CanEwb = "https://gsp.adaequare.com/test/enriched/ei/api/ewayapi";
        //public static string ExtQrCode = "https://gsp.adaequare.com/test/enriched/ei/others/extract/qr";
        //public static string ExtSignInv = "https://gsp.adaequare.com/test/enriched/ei/others/extract/invoice";
        //public static string GenQRImg = "https://gsp.adaequare.com/test/enriched/ei/others/qr/image";
        //public static string GetQRImg = "https://gsp.adaequare.com/test/enriched/ei/others/qr/image";
        //public static string GetEwbbyIrn = "https://gsp.adaequare.com/test/enriched/ei/api/ewaybill/irn?";


        //////Eway bill API URLS

        //public static string GenEwaybillApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi?action=GENEWAYBILL";
        //public static string VehUpdApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi?action=VEHEWB";
        //public static string GENCEWBApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi?action=GENCEWB";
        //public static string CANEWBApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi?action=CANEWB";
        //public static string REJEWBApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi?action=REJEWB";
        //public static string EXTENDVALIDITYApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi?action=EXTENDVALIDITY";
        //public static string REGENTRIPSHEETApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi?action=REGENTRIPSHEET";
        //public static string UPDATETRANSPORTERApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi?action=UPDATETRANSPORTER";
        //public static string GetEwayApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi/GetEwayBill?ewbNo=351001073457";
        //public static string GetEwayTransApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi/GetEwayBillsForTransporter?date=29/03/2018";
        //public static string GetEwayTransbyGSTINApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi/GetEwayBillsForTransporterByGstin?Gen_gstin=05AAACG2115R1ZN&date=29/03/2018";
        //public static string GetEwayOtherPartyApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi/GetEwayBillsofOtherParty?date=20/04/2018";
        //public static string GetEwayConsoApi = "https://gsp.adaequare.com/test/enriched/ewb/ewayapi/GetTripSheet?tripSheetNo=3710007099";
        //public static string GetEwayTransporterApi = "https://gsp.adaequare.com/test/enriched/ewb/master/GetTransporterDetails?trn_no=06AAACG2115R1ZN";
        //public static string GetEwayErrorlistApi = "https://gsp.adaequare.com/test/enriched/ewb/master/GetErrorList?";
        //public static string GetEwayhsnApi = "https://gsp.adaequare.com/test/enriched/ewb/master/GetHsnDetailsByHsnCode?hsncode=1001";
        //public static string GetEwayTaxPayerApi = "https://gsp.adaequare.com/test/enriched/ewb/master/GetGSTINDetails?GSTIN=05AAACG2115R1ZN";


        //Live API Urls

        public static string AuthApi = "https://gsp.adaequare.com/gsp/authenticate?action=GSP&grant_type=token";
        public static string GenIrnApi = "https://gsp.adaequare.com/enriched/ei/api/invoice";
        public static string CanIrnApi = "https://gsp.adaequare.com/enriched/ei/api/invoice/cancel";
        public static string GetInvbyIrn = "https://gsp.adaequare.com/enriched/ei/api/invoice/irn?";
        public static string Taxpayerdtl = "https://gsp.adaequare.com/enriched/ei/api/master/gstin?gstin=36GSPOHM281G1ZP";
        public static string GenEwbbyIrn = "https://gsp.adaequare.com/enriched/ei/api/ewaybill";
        public static string CanEwb = "https://gsp.adaequare.com/enriched/ei/api/ewayapi";
        public static string ExtQrCode = "https://gsp.adaequare.com/enriched/ei/others/extract/qr";
        public static string ExtSignInv = "https://gsp.adaequare.com/enriched/ei/others/extract/invoice";
        public static string GenQRImg = "https://gsp.adaequare.com/enriched/ei/others/qr/image";
        public static string GetQRImg = "https://gsp.adaequare.com/enriched/ei/others/qr/image";
        public static string GetEwbbyIrn = "https://gsp.adaequare.com/enriched/ei/api/ewaybill/irn?";


        ////Eway bill API URLS

        public static string GenEwaybillApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi?action=GENEWAYBILL";
        public static string VehUpdApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi?action=VEHEWB";
        public static string GENCEWBApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi?action=GENCEWB";
        public static string CANEWBApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi?action=CANEWB";
        public static string REJEWBApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi?action=REJEWB";
        public static string EXTENDVALIDITYApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi?action=EXTENDVALIDITY";
        public static string REGENTRIPSHEETApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi?action=REGENTRIPSHEET";
        public static string UPDATETRANSPORTERApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi?action=UPDATETRANSPORTER";
        public static string GetEwayApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi/GetEwayBill?ewbNo=351001073457";
        public static string GetEwayTransApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi/GetEwayBillsForTransporter?date=29/03/2018";
        public static string GetEwayTransbyGSTINApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi/GetEwayBillsForTransporterByGstin?Gen_gstin=05AAACG2115R1ZN&date=29/03/2018";
        public static string GetEwayOtherPartyApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi/GetEwayBillsofOtherParty?date=20/04/2018";
        public static string GetEwayConsoApi = "https://gsp.adaequare.com/enriched/ewb/ewayapi/GetTripSheet?tripSheetNo=3710007099";
        public static string GetEwayTransporterApi = "https://gsp.adaequare.com/enriched/ewb/master/GetTransporterDetails?trn_no=06AAACG2115R1ZN";
        public static string GetEwayErrorlistApi = "https://gsp.adaequare.com/enriched/ewb/master/GetErrorList?";
        public static string GetEwayhsnApi = "https://gsp.adaequare.com/enriched/ewb/master/GetHsnDetailsByHsnCode?hsncode=1001";
        public static string GetEwayTaxPayerApi = "https://gsp.adaequare.com/enriched/ewb/master/GetGSTINDetails?GSTIN=05AAACG2115R1ZN";


    }

    public class AccessTokenResult
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public string jti { get; set; }
    }
}
