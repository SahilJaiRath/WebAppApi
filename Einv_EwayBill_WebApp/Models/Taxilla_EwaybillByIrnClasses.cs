using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    public class Taxilla_EwaybillByIrnClasses
    {
        public static string Generate_EwaybillIrn_Json(DataTable dt, string sessionId, string access_token)
        {
            string msg = "";
            try
            {
                string calcdist = "0";


                List<EwayBillByIrnRoot> ewaybillIrn_obj = new List<EwayBillByIrnRoot>();

                if (dt.Rows[0]["TRAN_CATG"].ToString().StartsWith("EXP"))
                {
                    EwayBillByIrnRoot rootDtls = new EwayBillByIrnRoot
                    {
                        Irn = dt.Rows[0]["IRN"].ToString() == "" ? sessionId : dt.Rows[0]["IRN"].ToString(),
                        TransId = dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString(),
                        TransMode = dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString(),
                        TransDocNo = dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString(),
                        TransDocDt = dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"].ToString() == "" ? null : Convert.ToDateTime(dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"]).ToString("dd/MM/yyyy").Replace("-", "/"),
                        VehNo = dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString(),
                        Distance = Convert.ToInt32((dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "" || dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "0") ? calcdist : dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString()),
                        VehType = dt.Rows[0]["EWAY_TRANSPORTAR_VEHITYPE"].ToString(),
                        TransName = dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString(),
                        ExpShipDtls = Expship_details(dt),
                        //DispDtls = getdispatch_details(dt)

                    };
                    ewaybillIrn_obj.Add(rootDtls);
                }

                else if ((dt.Rows[0]["BILLTO_GSTIN"].ToString() == dt.Rows[0]["SHIPTO_GSTIN"].ToString()))
                {

                    if (dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "" || dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "0")
                    {
                        if (dt.Rows[0]["BILLFROM_PIN"].ToString() == dt.Rows[0]["BILLTO_PIN"].ToString())
                        {
                            calcdist = "20";
                        }
                        else
                        {
                            string Rdistval = DistanceResponseClass.GoogleDistAPI(Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLFROM_PIN"].ToString())), Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLTO_PIN"].ToString())));
                            calcdist = (Rdistval);
                            string commandText = "UPDATE einvoice_generate_temp SET EWAY_TRANSPORTAR_DISTANCE='" + calcdist + "' WHERE DOC_NO ='" + dt.Rows[0]["DOC_No"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, commandText);
                        }
                    }

                    EwayBillByIrnRoot rootDtls = new EwayBillByIrnRoot
                    {
                        Irn = dt.Rows[0]["IRN"].ToString() == "" ? sessionId : dt.Rows[0]["IRN"].ToString(),
                        TransId = dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString(),
                        TransMode = dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString(),
                        TransDocNo = dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString(),
                        TransDocDt = dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"].ToString() == "" ? null : Convert.ToDateTime(dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"]).ToString("dd/MM/yyyy").Replace("-", "/"),
                        VehNo = dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString(),
                        Distance = Convert.ToInt32((dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "" || dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "0") ? calcdist : dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString()),
                        VehType = dt.Rows[0]["EWAY_TRANSPORTAR_VEHITYPE"].ToString(),
                        TransName = dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString(),
                        //ExpShipDtls = Expship_details(dt),
                        //DispDtls = getdispatch_details(dt)

                    };
                    ewaybillIrn_obj.Add(rootDtls);
                }

                else if ((dt.Rows[0]["BILLFROM_GSTIN"].ToString() == dt.Rows[0]["SHIPFROM_GSTIN"].ToString()))
                {

                    if (dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "" || dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "0")
                    {
                        if (dt.Rows[0]["BILLFROM_PIN"].ToString() == dt.Rows[0]["BILLTO_PIN"].ToString())
                        {
                            calcdist = "20";
                        }
                        else
                        {
                            string Rdistval = DistanceResponseClass.GoogleDistAPI(Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLFROM_PIN"].ToString())), Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLTO_PIN"].ToString())));
                            calcdist = (Rdistval);
                            string commandText = "UPDATE einvoice_generate_temp SET EWAY_TRANSPORTAR_DISTANCE='" + calcdist + "' WHERE DOC_NO ='" + dt.Rows[0]["DOC_No"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, commandText);
                        }
                    }

                    EwayBillByIrnRoot rootDtls = new EwayBillByIrnRoot
                    {
                        Irn = dt.Rows[0]["IRN"].ToString() == "" ? sessionId : dt.Rows[0]["IRN"].ToString(),
                        TransId = dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString(),
                        TransMode = dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString(),
                        TransDocNo = dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString(),
                        TransDocDt = dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"].ToString() == "" ? null : Convert.ToDateTime(dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"]).ToString("dd/MM/yyyy").Replace("-", "/"),
                        VehNo = dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString(),
                        Distance = Convert.ToInt32((dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "" || dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "0") ? calcdist : dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString()),
                        VehType = dt.Rows[0]["EWAY_TRANSPORTAR_VEHITYPE"].ToString(),
                        TransName = dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString(),
                        //ExpShipDtls = Expship_details(dt),
                        //DispDtls = getdispatch_details(dt)

                    };
                    ewaybillIrn_obj.Add(rootDtls);
                }




                else
                {
                    if (dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "" || dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "0")
                    {
                        if (dt.Rows[0]["BILLFROM_PIN"].ToString() == dt.Rows[0]["SHIPTO_PIN"].ToString())
                        {
                            calcdist = "20";
                        }
                        else
                        {
                            string Rdistval = DistanceResponseClass.GoogleDistAPI(Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLFROM_PIN"].ToString())), Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["SHIPTO_PIN"].ToString())));
                            calcdist = (Rdistval);
                            string commandText = "UPDATE einvoice_generate_temp SET EWAY_TRANSPORTAR_DISTANCE='" + calcdist + "' WHERE DOC_NO ='" + dt.Rows[0]["DOC_No"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, commandText);
                        }
                    }

                    EwayBillByIrnRoot rootDtls = new EwayBillByIrnRoot
                    {
                        Irn = dt.Rows[0]["IRN"].ToString() == "" ? sessionId : dt.Rows[0]["IRN"].ToString(),
                        TransId = dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString(),
                        TransMode = dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString(),
                        TransDocNo = dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString(),
                        TransDocDt = dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"].ToString() == "" ? null : Convert.ToDateTime(dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"]).ToString("dd/MM/yyyy").Replace("-", "/"),
                        VehNo = dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString(),
                        Distance = Convert.ToInt32((dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "" || dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "0") ? calcdist : dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString()),
                        VehType = dt.Rows[0]["EWAY_TRANSPORTAR_VEHITYPE"].ToString(),
                        TransName = dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString(),
                        ExpShipDtls = Expship_details(dt),
                        DispDtls = getdispatch_details(dt)

                    };
                    ewaybillIrn_obj.Add(rootDtls);
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(ewaybillIrn_obj);
                string mystr = json.Substring(1, json.Length - 2);
                return msg = mystr;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog(ex.Message);
                msg = "";
                return msg;
            }
        }

        public static DispDtls getdispatch_details(DataTable dt)
        {
            //ClsDynamic.WriteLog("getdispatch_details  Called");
            DispDtls dispatch_details = new DispDtls
            {
                Nm = dt.Rows[0]["SHIPFROM_TRDNM"].ToString() == "" ? "" : dt.Rows[0]["SHIPFROM_TRDNM"].ToString(),              //Mandatory
                Addr1 = dt.Rows[0]["SHIPFROM_BNO"].ToString() + " " + dt.Rows[0]["SHIPFROM_BNM"].ToString() + " " + dt.Rows[0]["SHIPFROM_FLNO"].ToString() + " " + dt.Rows[0]["SHIPFROM_DST"].ToString(),     //Mandatory
                Addr2 = null,
                Loc = dt.Rows[0]["SHIPFROM_LOC"].ToString() == "" ? "" : dt.Rows[0]["SHIPFROM_LOC"].ToString(),                                       //Mandatory
                Pin = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["SHIPFROM_PIN"].ToString() == "" ? "201301" : dt.Rows[0]["SHIPFROM_PIN"].ToString())),    //Mandatory
                Stcd = dt.Rows[0]["SHIPFROM_STCD"].ToString() == "" ? "" : dt.Rows[0]["SHIPFROM_STCD"].ToString()                                        //Mandatory
            };

            return dispatch_details;
        }

        public static ExpShipDtls Expship_details(DataTable dt)
        {
            //ClsDynamic.WriteLog("getship_details  Called");
            ExpShipDtls ship_details = new ExpShipDtls
            {
                Addr1 = dt.Rows[0]["SHIPTO_BNO"].ToString() + " " + dt.Rows[0]["SHIPTO_BNM"].ToString() + " " + dt.Rows[0]["SHIPTO_FLNO"].ToString() + " " + dt.Rows[0]["SHIPTO_DST"].ToString(),         //Mandatory
                Addr2 = null,
                Loc = dt.Rows[0]["SHIPTO_LOC"].ToString(),                                       //Mandatory
                Pin = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["SHIPTO_PIN"].ToString() == "" ? "201301" : dt.Rows[0]["SHIPTO_PIN"].ToString())),    //Mandatory
                Stcd = dt.Rows[0]["SHIPTO_STCD"].ToString()                                       //Mandatory
            };

            return ship_details;
        }

        public static string Cancel_Ewaybill_Json(DataTable dt, string sessionId, string access_token)
        {
            string msg = "";
            List<CancelEwaybillIRNRoot> ewaybillIrn_obj = new List<CancelEwaybillIRNRoot>();
            CancelEwaybillIRNRoot rootDtls = new CancelEwaybillIRNRoot
            {
                ewbNo = Convert.ToInt64(dt.Rows[0]["EWBNO"].ToString() == "" ? "111000609282" : dt.Rows[0]["EWBNO"].ToString()),
                cancelRsnCode = 2,
                cancelRmrk = "Cancelled the order"
            };
            ewaybillIrn_obj.Add(rootDtls);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ewaybillIrn_obj);
            string mystr = json.Substring(1, json.Length - 2);
            return msg = mystr;
        }

        public static string ExtractQR_Code_Json(DataTable dt, string sessionId, string access_token)
        {
            string msg = "";
            List<ExtractQRRoot> ewaybillIrn_obj = new List<ExtractQRRoot>();
            ExtractQRRoot rootDtls = new ExtractQRRoot
            {
                data = dt.Rows[0]["SIGNEDQRCODE"].ToString()
            };
            ewaybillIrn_obj.Add(rootDtls);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ewaybillIrn_obj);
            string mystr = json.Substring(1, json.Length - 2);
            return msg = mystr;
        }

        public static string Extract_SignedInvoice_Json(DataTable dt, string sessionId, string access_token)
        {
            string msg = "";
            List<ExtractQRRoot> ewaybillIrn_obj = new List<ExtractQRRoot>();
            ExtractQRRoot rootDtls = new ExtractQRRoot
            {
                data = dt.Rows[0]["SIGNEDINVOICE"].ToString()
            };
            ewaybillIrn_obj.Add(rootDtls);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ewaybillIrn_obj);
            string mystr = json.Substring(1, json.Length - 2);
            return msg = mystr;

        }
    }

    public class EwayBillByIrnRoot
    {
        public string Irn { get; set; }
        public int Distance { get; set; }
        public string TransMode { get; set; }
        public string TransId { get; set; }
        public string TransName { get; set; }
        public string TransDocDt { get; set; }
        public string TransDocNo { get; set; }
        public string VehNo { get; set; }
        public string VehType { get; set; }
        public ExpShipDtls ExpShipDtls { get; set; }
        public DispDtls DispDtls { get; set; }
    }

    public class ExpShipDtls
    {
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
    }

    public class SuccessResult_EwbByIrn
    {
        public long EwayBillNo { get; set; }
        public string EwayBillDate { get; set; }
        public string ValidUpto { get; set; }
    }

    public class SuccessRoot_EwbByIrn
    {
        public bool success { get; set; }
        public string message { get; set; }
        public SuccessResult_EwbByIrn result { get; set; }
    }

    public class CancelEwaybillIRNRoot
    {
        public long ewbNo { get; set; }
        public int cancelRsnCode { get; set; }
        public string cancelRmrk { get; set; }
    }

    public class SuccessResult_CEWBIRN
    {
        public long ewayBillNo { get; set; }
        public string cancelDate { get; set; }
    }

    public class SuccessRoot_CEWBIRN
    {
        public bool success { get; set; }
        public string message { get; set; }
        public SuccessResult_CEWBIRN result { get; set; }
    }

    public class ExtractQRRoot
    {
        public string data { get; set; }
    }

    public class SuccessResult_EXTQR
    {
        public string Irn { get; set; }
        public string SellerGstin { get; set; }
        public string DocTyp { get; set; }
        public double TotInvVal { get; set; }
        public string BuyerGstin { get; set; }
        public string DocDt { get; set; }
        public string DocNo { get; set; }
        public string MainHsnCode { get; set; }
        public int ItemCnt { get; set; }
    }

    public class SuccessRoot_EXTQR
    {
        public bool success { get; set; }
        public string message { get; set; }
        public SuccessResult_EXTQR result { get; set; }
    }


    public class IrnEwaybill_successResult
    {
        public string EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
    }

    public class IrnEwaybill_successRoot
    {
        public bool success { get; set; }
        public string message { get; set; }       
        public IrnEwaybill_successResult result { get; set; }
    }

    public class GetEwaybillResult
    {
        public long EwbNo { get; set; }
        public string Status { get; set; }
        public string GenGstin { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
    }

    public class GetEwaybillRoot
    {
        public bool success { get; set; }
        public string message { get; set; }
        public GetEwaybillResult result { get; set; }
    }
}
