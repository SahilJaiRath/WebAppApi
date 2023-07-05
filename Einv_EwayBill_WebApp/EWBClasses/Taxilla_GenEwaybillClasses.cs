using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.EWBClasses
{
    public class Taxilla_GenEwaybillClasses
    {
        public static string GenerateJsonFile(DataTable dt, string access_token)
        {
            string msg = "";
            List<GenEwaybject> ewaobj = new List<GenEwaybject>();
            GenEwaybject rootDtls = new GenEwaybject
            {
                supplyType = dt.Rows[0]["SUPPLYTYPE"].ToString() == "" ? "O" : dt.Rows[0]["SUPPLYTYPE"].ToString(),
                subSupplyType = dt.Rows[0]["SUBSUPPLYTYPE"].ToString() == "" ? "" : dt.Rows[0]["SUBSUPPLYTYPE"].ToString(),
                subSupplyDesc = dt.Rows[0]["SUBSUPPLYDESC"].ToString() == "" ? "" : dt.Rows[0]["SUBSUPPLYDESC"].ToString(),
                //SUBSUPPLYDESC
                docType = dt.Rows[0]["DOCTYPE"].ToString() == "" ? "" : dt.Rows[0]["DOCTYPE"].ToString(),
                docNo = dt.Rows[0]["DOCNO"].ToString() == "" ? "" : dt.Rows[0]["DOCNO"].ToString(),
                docDate = dt.Rows[0]["DOCDATE"].ToString() == "" ? "" : dt.Rows[0]["DOCDATE"].ToString(),
                fromGstin = dt.Rows[0]["FROMGSTIN"].ToString() == "" ? "" : dt.Rows[0]["FROMGSTIN"].ToString(),
                fromTrdName = dt.Rows[0]["FROMTRDNAME"].ToString() == "" ? "" : dt.Rows[0]["FROMTRDNAME"].ToString(),
                fromAddr1 = dt.Rows[0]["FROMADDR1"].ToString() == "" ? "" : dt.Rows[0]["FROMADDR1"].ToString(),
                fromAddr2 = dt.Rows[0]["FROMADDR2"].ToString() == "" ? "" : dt.Rows[0]["FROMADDR2"].ToString(),
                fromPlace = dt.Rows[0]["FROMPLACE"].ToString() == "" ? "" : dt.Rows[0]["FROMPLACE"].ToString(),
                fromPincode = Convert.ToInt32(dt.Rows[0]["FROMPINCODE"].ToString() == "" ? "0" : dt.Rows[0]["FROMPINCODE"].ToString()),
                fromStateCode = Convert.ToInt32(dt.Rows[0]["FROMSTATECODE"].ToString() == "" ? "0" : dt.Rows[0]["FROMSTATECODE"].ToString()),
                actFromStateCode = Convert.ToInt32(dt.Rows[0]["ACTFROMSTATECODE"].ToString() == "" ? "0" : dt.Rows[0]["ACTFROMSTATECODE"].ToString()),//
                toGstin = dt.Rows[0]["TOGSTIN"].ToString() == "" ? "" : dt.Rows[0]["TOGSTIN"].ToString(),//
                toTrdname = dt.Rows[0]["TOTRDNAME"].ToString() == "" ? "" : dt.Rows[0]["TOTRDNAME"].ToString(),
                toAddr1 = dt.Rows[0]["TOADDR1"].ToString() == "" ? "" : dt.Rows[0]["TOADDR1"].ToString(),
                toAddr2 = dt.Rows[0]["TOADDR2"].ToString() == "" ? "" : dt.Rows[0]["TOADDR2"].ToString(),
                toPlace = dt.Rows[0]["TOPLACE"].ToString() == "" ? "" : dt.Rows[0]["TOPLACE"].ToString(),
                toPincode = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["TOPINCODE"].ToString() == "" ? "0" : dt.Rows[0]["TOPINCODE"].ToString())),
                toStateCode = Convert.ToInt32(dt.Rows[0]["TOSTATECODE"].ToString() == "" ? "0" : dt.Rows[0]["TOSTATECODE"].ToString()),
                actToStateCode = Convert.ToInt32(dt.Rows[0]["ACTTOSTATECODE"].ToString() == "" ? "0" : dt.Rows[0]["ACTTOSTATECODE"].ToString()),
                totalValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["TOTALVALUE"].ToString() == "" ? "0" : dt.Rows[0]["TOTALVALUE"].ToString())),
                cgstValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["CGSTVALUE"].ToString() == "" ? "0" : dt.Rows[0]["CGSTVALUE"].ToString())),
                sgstValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["SGSTVALUE"].ToString() == "" ? "0" : dt.Rows[0]["SGSTVALUE"].ToString())),
                igstValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["IGSTVALUE"].ToString() == "" ? "0" : dt.Rows[0]["IGSTVALUE"].ToString())),
                cessValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["CESSVALUE"].ToString() == "" ? "0" : dt.Rows[0]["CESSVALUE"].ToString())),
                transMode = dt.Rows[0]["TRANSMODE"].ToString() == "" ? "" : dt.Rows[0]["TRANSMODE"].ToString(),
                transDistance = dt.Rows[0]["TRANSDISTANCE"].ToString() == "" ? "" : dt.Rows[0]["TRANSDISTANCE"].ToString(),
                transporterName = dt.Rows[0]["TRANSPORTERNAME"].ToString() == "" ? "" : dt.Rows[0]["TRANSPORTERNAME"].ToString(),
                transporterId = dt.Rows[0]["TRANSPORTERID"].ToString() == "" ? "" : dt.Rows[0]["TRANSPORTERID"].ToString(),
                transDocNo = dt.Rows[0]["TRANSDOCNO"].ToString() == "" ? "" : dt.Rows[0]["TRANSDOCNO"].ToString(),
                transDocDate = dt.Rows[0]["TRANSDOCDATE"].ToString() == "" ? "" : dt.Rows[0]["TRANSDOCDATE"].ToString(),
                vehicleNo = dt.Rows[0]["VEHICLENO"].ToString() == "" ? "" : dt.Rows[0]["VEHICLENO"].ToString(),
                vehicleType = dt.Rows[0]["VEHICLETYPE"].ToString() == "" ? "" : dt.Rows[0]["VEHICLETYPE"].ToString(),
                totInvValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["TOTINVVALUE"].ToString() == "" ? "0" : dt.Rows[0]["TOTINVVALUE"].ToString())),
                transactionType = Convert.ToInt32(dt.Rows[0]["TRANSACTIONTYPE"].ToString() == "" ? "1" : dt.Rows[0]["TRANSACTIONTYPE"].ToString()),               
                otherValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["OTHERVALUE"].ToString() == "" ? "0" : dt.Rows[0]["OTHERVALUE"].ToString())),
                cessNonAdvolValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["CESSNONADVOLVALUE"].ToString() == "" ? "0" : dt.Rows[0]["CESSNONADVOLVALUE"].ToString())),

                itemList = getitem_list(dt),
            };
            ewaobj.Add(rootDtls);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ewaobj);
            string mystr = json.Substring(1, json.Length - 2);
            return msg = mystr;

        }

        public static string EWBJsonFile(DataTable dt, string access_token)
        {
            string msg = "";
            try
            {
                List<GenEwaybject> ewaobj = new List<GenEwaybject>();
                GenEwaybject rootDtls = new GenEwaybject
                {
                    //supplyType = dt.Rows[0]["SUPPLYTYPE"].ToString() == "" ? "O" : dt.Rows[0]["SUPPLYTYPE"].ToString(),
                    supplyType = "O",
                    subSupplyType = dt.Rows[0]["SUBSUPPLYTYPE"].ToString() == "" ? "5" : dt.Rows[0]["SUBSUPPLYTYPE"].ToString(),
                    subSupplyDesc = dt.Rows[0]["SUBSUPPLYDESC"].ToString() == "" ? "Others" : dt.Rows[0]["SUBSUPPLYDESC"].ToString(),
                    //SUBSUPPLYDESC
                    docType = dt.Rows[0]["DOC_TYP"].ToString() == "" ? "CHL" : dt.Rows[0]["DOC_TYP"].ToString(),
                    docNo = dt.Rows[0]["DOC_NO"].ToString() == "" ? "" : dt.Rows[0]["DOC_NO"].ToString(),
                    docDate = Convert.ToDateTime(dt.Rows[0]["DOC_DT"]).ToString("dd/MM/yyyy").Replace("-","/"),
                    fromGstin = dt.Rows[0]["BILLFROM_GSTIN"].ToString() == "" ? "" : dt.Rows[0]["BILLFROM_GSTIN"].ToString(),
                    fromTrdName = dt.Rows[0]["BILLFROM_TRDNM"].ToString() == "" ? "" : dt.Rows[0]["BILLFROM_TRDNM"].ToString(),
                    fromAddr1 = dt.Rows[0]["BILLFROM_BNO"].ToString() == "" ? "" : dt.Rows[0]["BILLFROM_BNO"].ToString(),
                    fromAddr2 = dt.Rows[0]["BILLFROM_BNO"].ToString() == "" ? "" : dt.Rows[0]["BILLFROM_BNO"].ToString(),
                    fromPlace = dt.Rows[0]["BILLFROM_LOC"].ToString() == "" ? "" : dt.Rows[0]["BILLFROM_LOC"].ToString(),
                    fromPincode = Convert.ToInt32(dt.Rows[0]["BILLFROM_PIN"].ToString() == "" ? "0" : dt.Rows[0]["BILLFROM_PIN"].ToString()),
                    fromStateCode = Convert.ToInt32(dt.Rows[0]["BILLFROM_STCD"].ToString() == "" ? "0" : dt.Rows[0]["BILLFROM_STCD"].ToString()),
                    actFromStateCode = Convert.ToInt32(dt.Rows[0]["BILLFROM_STCD"].ToString() == "" ? "0" : dt.Rows[0]["BILLFROM_STCD"].ToString()),//
                    toGstin = dt.Rows[0]["BILLTO_GSTIN"].ToString() == "" ? "" : dt.Rows[0]["BILLTO_GSTIN"].ToString(),//
                    toTrdname = dt.Rows[0]["BILLTO_TRDNM"].ToString() == "" ? "" : dt.Rows[0]["BILLTO_TRDNM"].ToString(),
                    toAddr1 = dt.Rows[0]["BILLTO_BNO"].ToString() == "" ? "" : dt.Rows[0]["BILLTO_BNO"].ToString(),
                    toAddr2 = dt.Rows[0]["BILLTO_BNO"].ToString() == "" ? "" : dt.Rows[0]["BILLTO_BNO"].ToString(),
                    toPlace = dt.Rows[0]["BILLTO_LOC"].ToString() == "" ? "" : dt.Rows[0]["BILLTO_LOC"].ToString(),
                    toPincode = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLTO_PIN"].ToString() == "" ? "0" : dt.Rows[0]["BILLTO_PIN"].ToString())),
                    toStateCode = Convert.ToInt32(dt.Rows[0]["BILLTO_STCD"].ToString() == "" ? "0" : dt.Rows[0]["BILLTO_STCD"].ToString()),
                    actToStateCode = Convert.ToInt32(dt.Rows[0]["BILLTO_STCD"].ToString() == "" ? "0" : dt.Rows[0]["BILLTO_STCD"].ToString()),

                    totalValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["VAL_ASSVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_ASSVAL"].ToString())),
                    cgstValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["VAL_CGSTVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_CGSTVAL"].ToString())),
                    sgstValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["VAL_SGSTVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_SGSTVAL"].ToString())),
                    igstValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["VAL_IGSTVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_IGSTVAL"].ToString())),
                    cessValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["VAL_CESVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_CESVAL"].ToString())),
                    otherValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["VAL_OTHCHRG"].ToString() == "" ? "0" : dt.Rows[0]["VAL_OTHCHRG"].ToString())),
                    cessNonAdvolValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["VAL_CESNONADVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_CESNONADVAL"].ToString())),


                    transMode = dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString() == "" ? "" : dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString(),
                    transDistance = dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "" ? "" : dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString(),
                    transporterName = dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString() == "" ? "" : dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString(),
                    transporterId = dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString() == "" ? "" : dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString(),
                    transDocNo = dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString() == "" ? "" : dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString(),
                    transDocDate = dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"].ToString() == "" ? "" : dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"].ToString(),
                    vehicleNo = dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString() == "" ? "" : dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString(),
                    vehicleType = dt.Rows[0]["EWAY_TRANSPORTAR_VEHITYPE"].ToString() == "" ? "" : dt.Rows[0]["EWAY_TRANSPORTAR_VEHITYPE"].ToString(),

                    totInvValue = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["VAL_TOTINVVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_TOTINVVAL"].ToString())),
                    transactionType = Convert.ToInt32(dt.Rows[0]["EWB_TRANSACTIONTYPE"].ToString() == "" ? "1" : dt.Rows[0]["EWB_TRANSACTIONTYPE"].ToString()),
                    itemList = getitem_list(dt),
                };
                ewaobj.Add(rootDtls);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(ewaobj);
                string mystr = json.Substring(1, json.Length - 2);
                msg = mystr;
            }
            catch(Exception ex)
            {                
                throw;
                msg = "";
            }
            return msg;

        }

        public static List<ItemList> getitem_list(DataTable dt)
        {
            List<ItemList> itm = new List<ItemList>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itm.Add(new ItemList
                {
                    productName = dt.Rows[i]["ITEM_PRDNM"].ToString() == "" ? "" : dt.Rows[i]["ITEM_PRDNM"].ToString(),
                    productDesc = dt.Rows[i]["ITEM_PRDDESC"].ToString() == "" ? "" : dt.Rows[i]["ITEM_PRDDESC"].ToString(),
                    hsnCode = dt.Rows[i]["ITEM_HSNCD"].ToString() == "" ? "" : dt.Rows[i]["ITEM_HSNCD"].ToString(),
                    quantity = Convert.ToInt64(Convert.ToDouble(dt.Rows[i]["ITEM_QTY"].ToString())),
                    qtyUnit = dt.Rows[i]["ITEM_UNIT"].ToString() == "" ? "" : dt.Rows[i]["ITEM_UNIT"].ToString(),
                    cgstRate = Convert.ToInt64(Convert.ToDouble(dt.Rows[i]["ITEM_CGSTRT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_CGSTRT"].ToString())),
                    sgstRate = Convert.ToInt64(Convert.ToDouble(dt.Rows[i]["ITEM_CGSTRT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_CGSTRT"].ToString())),
                    igstRate = Convert.ToInt64(Convert.ToDouble(dt.Rows[i]["ITEM_IGSTRT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_IGSTRT"].ToString())),
                    cessRate = Convert.ToInt64(dt.Rows[i]["ITEM_CESRT"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_CESRT"].ToString()),
                    cessNonAdvol = Convert.ToInt64(Convert.ToDouble(dt.Rows[i]["ITEM_CESNONADVAL"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_CESNONADVAL"].ToString())),
                    taxableAmount = Convert.ToInt64(Convert.ToDouble(dt.Rows[i]["ITEM_TOTITEMVAL"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_TOTITEMVAL"].ToString()))
                });
            }
            return itm;
        }
    }
    public class ItemList
    {
        public string productName { get; set; }
        public string productDesc { get; set; }
        public string hsnCode { get; set; }
        public double quantity { get; set; }
        public string qtyUnit { get; set; }
        public double taxableAmount { get; set; }
        public double sgstRate { get; set; }
        public double cgstRate { get; set; }
        public double igstRate { get; set; }
        public double cessRate { get; set; }
        public double cessNonAdvol { get; set; }
    }

    public class GenEwaybject
    {
        public string supplyType { get; set; }
        public string subSupplyType { get; set; }
        public string subSupplyDesc { get; set; }
        public string docType { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string fromGstin { get; set; }
        public string fromTrdName { get; set; }
        public string fromAddr1 { get; set; }
        public string fromAddr2 { get; set; }
        public string fromPlace { get; set; }
        public int fromPincode { get; set; }
        public int fromStateCode { get; set; }
        public int actFromStateCode { get; set; }
        public string toGstin { get; set; }
        public string toTrdname { get; set; }
        public string toAddr1 { get; set; }
        public string toAddr2 { get; set; }
        public string toPlace { get; set; }
        public int toPincode { get; set; }
        public int toStateCode { get; set; }
        public int actToStateCode { get; set; }
        public int totalValue { get; set; }
        public double cgstValue { get; set; }
        public double sgstValue { get; set; }
        public double igstValue { get; set; }
        public double cessValue { get; set; }
        public string transMode { get; set; }
        public string transDistance { get; set; }
        public string transporterName { get; set; }
        public string transporterId { get; set; }
        public string transDocNo { get; set; }
        public string transDocDate { get; set; }
        public string vehicleNo { get; set; }
        public string vehicleType { get; set; }
        public int totInvValue { get; set; }
        public int transactionType { get; set; }
        public string dispatchFromGSTIN { get; set; }
        public string dispatchFromTradeName { get; set; }
        public string shipToGSTIN { get; set; }
        public string shipToTradeName { get; set; }
        public int otherValue { get; set; }
        public int cessNonAdvolValue { get; set; }
        public List<ItemList> itemList { get; set; }
    }

    public class EwaySuccessResult
    {
        public string ewayBillDate { get; set; }
        public long ewayBillNo { get; set; }
        public string alert { get; set; }
        public string validUpto { get; set; }
    }

    public class EwaySuccessRoot
    {
        public bool success { get; set; }
        public EwaySuccessResult result { get; set; }
        public string message { get; set; }
    }
}

