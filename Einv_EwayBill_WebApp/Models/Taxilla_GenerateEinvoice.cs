using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    public class Taxilla_GenerateEinvoice
    {
        //public static string GenerateIRNJsonFile(DataTable dt,string EINVEWB)
        //{

        //    string msg = "";
        //    if (dt.Rows.Count > 0)
        //    {
        //        List<GenIrnRoot> customers = new List<GenIrnRoot>();

        //        //for (int j = 0; j < results.Rows.Count; j++)
        //        // {
        //        if (EINVEWB == "Y")
        //        {
        //            GenIrnRoot TranDtls = new GenIrnRoot
        //            {
        //                Version = "1.1",
        //                TranDtls = GetTranGetails(dt),
        //                DocDtls = GetDocumentDetails(dt),
        //                SellerDtls = GetSellerDetails(dt),
        //                BuyerDtls = GetBuyerDetails(dt),
        //                DispDtls = getdispatch_details(dt),
        //                ShipDtls = getship_details(dt),
        //                ItemList = getitem_list(dt),
        //                ValDtls = getvalue_details(dt),
        //                PayDtls = getpayment_details(dt),
        //                RefDtls = getreference_details(dt),
        //                AddlDocDtls = get_additional_document(dt),
        //                ExpDtls = getexport_details(dt),
        //                EwbDtls = getewaybill_details(dt),

        //            };
        //            customers.Add(TranDtls);
        //        }
        //        else
        //        {
        //            GenIrnRoot TranDtls = new GenIrnRoot
        //            {
        //                Version = "1.1",
        //                TranDtls = GetTranGetails(dt),
        //                DocDtls = GetDocumentDetails(dt),
        //                SellerDtls = GetSellerDetails(dt),
        //                BuyerDtls = GetBuyerDetails(dt),

        //                //********************Not Required For einvoice*******************8

        //                //DispDtls = getdispatch_details(dt),
        //                //ShipDtls = getship_details(dt),
        //                //ExpDtls = getexport_details(dt),
        //                //PayDtls = getpayment_details(dt),
        //                //RefDtls = getreference_details(dt),
        //                //AddlDocDtls = get_additional_document(dt),
        //                //EwbDtls = getewaybill_details(dt),

        //                //********************End*******************8

        //                ItemList = getitem_list(dt),
        //                ValDtls = getvalue_details(dt),

        //            };
        //            customers.Add(TranDtls);
        //        }


        //        //}

        //        //var json = new JavaScriptSerializer().Serialize(customers);
        //        var json = Newtonsoft.Json.JsonConvert.SerializeObject(customers);
        //        string mystr = json.Substring(1, json.Length - 2);
        //        return msg = mystr;
        //    }
        //    else
        //    {
        //        return msg = "No Data Found";
        //    }
            
        //}

        public static string GenerateIRNJsonFile(DataTable dt, string EINVEWB)
        {
            string msg = "";
            try
            {
                List<GenIrnRoot> customers = new List<GenIrnRoot>();

                if (dt.Rows[0]["TRAN_CATG"].ToString().StartsWith("EXP"))
                {
                    GenIrnRoot TranDtls = new GenIrnRoot
                    {
                        //Version = Convert.ToString(dt.Rows[0]["VERSION"]),
                        Version = "1.1",
                        TranDtls = GetTranGetails(dt),
                        DocDtls = GetDocumentDetails(dt),
                        SellerDtls = GetSellerDetails(dt),
                        BuyerDtls = GetBuyerDetails(dt),
                        //DispDtls = getdispatch_details(dt),
                        //ShipDtls = getship_details(dt),
                        ItemList = getitem_list(dt),
                        ValDtls = getvalue_details(dt),
                        PayDtls = getpayment_details(dt),
                        //RefDtls = getreference_details(dt),
                        AddlDocDtls = get_additional_document(dt),
                        ExpDtls = getexport_details(dt),
                        //EwbDtls = getewaybill_details(dt),

                    };
                    customers.Add(TranDtls);
                }

                else if (((dt.Rows[0]["BILLTO_GSTIN"].ToString() == dt.Rows[0]["SHIPTO_GSTIN"].ToString()) && (dt.Rows[0]["BILLFROM_GSTIN"].ToString() == dt.Rows[0]["SHIPFROM_GSTIN"].ToString())) && EINVEWB == "Y")
                {
                    if(dt.Rows[0]["BILLTO_BNO"].ToString() == dt.Rows[0]["SHIPTO_BNM"].ToString())
                        {
                        GenIrnRoot TranDtls = new GenIrnRoot
                        {
                            //Version = Convert.ToString(dt.Rows[0]["VERSION"]),
                            Version = "1.1",
                            TranDtls = GetTranGetails(dt),
                            DocDtls = GetDocumentDetails(dt),
                            SellerDtls = GetSellerDetails(dt),
                            BuyerDtls = GetBuyerDetails(dt),
                            //DispDtls = getdispatch_details(dt),
                            //ShipDtls = getship_details(dt),
                            ItemList = getitem_list(dt),
                            ValDtls = getvalue_details(dt),
                            PayDtls = getpayment_details(dt),
                            //RefDtls = getreference_details(dt),
                            AddlDocDtls = get_additional_document(dt),
                            ExpDtls = getexport_details(dt),
                            EwbDtls = getewaybill_details(dt),

                        };
                        customers.Add(TranDtls);
                    }
                    else
                    {
                        GenIrnRoot TranDtls = new GenIrnRoot
                        {
                            //Version = Convert.ToString(dt.Rows[0]["VERSION"]),
                            Version = "1.1",
                            TranDtls = GetTranGetails(dt),
                            DocDtls = GetDocumentDetails(dt),
                            SellerDtls = GetSellerDetails(dt),
                            BuyerDtls = GetBuyerDetails(dt),
                           // DispDtls = getdispatch_details(dt),
                            ShipDtls = getship_details(dt),
                            ItemList = getitem_list(dt),
                            ValDtls = getvalue_details(dt),
                            PayDtls = getpayment_details(dt),
                            //RefDtls = getreference_details(dt),
                            AddlDocDtls = get_additional_document(dt),
                            ExpDtls = getexport_details(dt),
                            EwbDtls = getewaybill_details(dt),

                        };
                        customers.Add(TranDtls);
                    }
                   
                    
                }
                else if (((dt.Rows[0]["BILLTO_GSTIN"].ToString() != dt.Rows[0]["SHIPTO_GSTIN"].ToString()) && (dt.Rows[0]["BILLFROM_GSTIN"].ToString() != dt.Rows[0]["SHIPFROM_GSTIN"].ToString()) ) && EINVEWB == "Y")
                {
                    GenIrnRoot TranDtls = new GenIrnRoot
                    {
                        //Version = Convert.ToString(dt.Rows[0]["VERSION"]),
                        Version = "1.1",
                        TranDtls = GetTranGetails(dt),
                        DocDtls = GetDocumentDetails(dt),
                        SellerDtls = GetSellerDetails(dt),
                        BuyerDtls = GetBuyerDetails(dt),
                        DispDtls = getdispatch_details(dt),
                        ShipDtls = getship_details(dt),
                        ItemList = getitem_list(dt),
                        ValDtls = getvalue_details(dt),
                        PayDtls = getpayment_details(dt),
                        //RefDtls = getreference_details(dt),
                        AddlDocDtls = get_additional_document(dt),
                        ExpDtls = getexport_details(dt),
                        EwbDtls = getewaybill_details(dt),

                    };
                    customers.Add(TranDtls);
                }

                else if (((dt.Rows[0]["BILLTO_GSTIN"].ToString() == dt.Rows[0]["SHIPTO_GSTIN"].ToString()) && (dt.Rows[0]["BILLFROM_GSTIN"].ToString() != dt.Rows[0]["SHIPFROM_GSTIN"].ToString()) ) && EINVEWB == "Y")
                {
                    if (dt.Rows[0]["BILLTO_BNO"].ToString() == dt.Rows[0]["SHIPTO_BNM"].ToString())
                    {
                        GenIrnRoot TranDtls = new GenIrnRoot
                        {
                            //Version = Convert.ToString(dt.Rows[0]["VERSION"]),
                            Version = "1.1",
                            TranDtls = GetTranGetails(dt),
                            DocDtls = GetDocumentDetails(dt),
                            SellerDtls = GetSellerDetails(dt),
                            BuyerDtls = GetBuyerDetails(dt),
                              DispDtls = getdispatch_details(dt),
                            //ShipDtls = getship_details(dt),
                            ItemList = getitem_list(dt),
                            ValDtls = getvalue_details(dt),
                            PayDtls = getpayment_details(dt),
                            //RefDtls = getreference_details(dt),
                            AddlDocDtls = get_additional_document(dt),
                            ExpDtls = getexport_details(dt),
                            EwbDtls = getewaybill_details(dt),

                        };
                        customers.Add(TranDtls);
                    }
                    else
                    {
                        GenIrnRoot TranDtls = new GenIrnRoot
                        {
                            //Version = Convert.ToString(dt.Rows[0]["VERSION"]),
                            Version = "1.1",
                            TranDtls = GetTranGetails(dt),
                            DocDtls = GetDocumentDetails(dt),
                            SellerDtls = GetSellerDetails(dt),
                            BuyerDtls = GetBuyerDetails(dt),
                            DispDtls = getdispatch_details(dt),
                            ShipDtls = getship_details(dt),
                            ItemList = getitem_list(dt),
                            ValDtls = getvalue_details(dt),
                            PayDtls = getpayment_details(dt),
                            //RefDtls = getreference_details(dt),
                            AddlDocDtls = get_additional_document(dt),
                            ExpDtls = getexport_details(dt),
                            EwbDtls = getewaybill_details(dt),

                        };
                        customers.Add(TranDtls);

                    }
                       
                }
                else if (((dt.Rows[0]["BILLTO_GSTIN"].ToString() != dt.Rows[0]["SHIPTO_GSTIN"].ToString()) && (dt.Rows[0]["BILLFROM_GSTIN"].ToString() == dt.Rows[0]["SHIPFROM_GSTIN"].ToString())) && EINVEWB == "Y")
                {
                    GenIrnRoot TranDtls = new GenIrnRoot
                    {
                        //Version = Convert.ToString(dt.Rows[0]["VERSION"]),
                        Version = "1.1",
                        TranDtls = GetTranGetails(dt),
                        DocDtls = GetDocumentDetails(dt),
                        SellerDtls = GetSellerDetails(dt),
                        BuyerDtls = GetBuyerDetails(dt),
                       // DispDtls = getdispatch_details(dt),
                        ShipDtls = getship_details(dt),
                        ItemList = getitem_list(dt),
                        ValDtls = getvalue_details(dt),
                        PayDtls = getpayment_details(dt),
                        //RefDtls = getreference_details(dt),
                        AddlDocDtls = get_additional_document(dt),
                        ExpDtls = getexport_details(dt),
                        EwbDtls = getewaybill_details(dt),

                    };
                    customers.Add(TranDtls);
                }


                else if (EINVEWB == "Y")
                {
                    GenIrnRoot TranDtls = new GenIrnRoot
                    {
                        //Version = Convert.ToString(dt.Rows[0]["VERSION"]),
                        Version = "1.1",
                        TranDtls = GetTranGetails(dt),
                        DocDtls = GetDocumentDetails(dt),
                        SellerDtls = GetSellerDetails(dt),
                        BuyerDtls = GetBuyerDetails(dt),
                        DispDtls = getdispatch_details(dt),
                        ShipDtls = getship_details(dt),
                        ItemList = getitem_list(dt),
                        ValDtls = getvalue_details(dt),
                        PayDtls = getpayment_details(dt),
                        //RefDtls = getreference_details(dt),
                        AddlDocDtls = get_additional_document(dt),
                        ExpDtls = getexport_details(dt),
                        EwbDtls = getewaybill_details(dt),

                    };
                    customers.Add(TranDtls);
                }
                else
                {
                    GenIrnRoot TranDtls = new GenIrnRoot
                    {
                        Version = "1.1",
                        TranDtls = GetTranGetails(dt),
                        DocDtls = GetDocumentDetails(dt),
                        SellerDtls = GetSellerDetails(dt),
                        BuyerDtls = GetBuyerDetails(dt),
                        ItemList = getitem_list(dt),
                        ValDtls = getvalue_details(dt),
                        //DispDtls = getdispatch_details(dt),
                        //ShipDtls = getship_details(dt),

                    };
                    customers.Add(TranDtls);
                }

                //var json = new JavaScriptSerializer().Serialize(customers);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(customers);
                string mystr = json.Substring(1, json.Length - 2);

                ClsDynamic.JsonLog(mystr, dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                return msg = mystr;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog(ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                return msg = "";
            }
        }

        public static TranDtls GetTranGetails(DataTable dt)
        {
            try
            {
                TranDtls TranDtls = new TranDtls
                {
                    TaxSch = dt.Rows[0]["TAXSCH"].ToString() == "" ? "GST" : dt.Rows[0]["TAXSCH"].ToString(),
                    SupTyp = dt.Rows[0]["TRAN_CATG"].ToString(),
                    RegRev = dt.Rows[0]["TRAN_ECMTRN"].ToString(),
                    EcmGstin = dt.Rows[0]["ecommerce_gstin"].ToString() == "" ? null : dt.Rows[0]["ecommerce_gstin"].ToString(),
                    IgstOnIntra = dt.Rows[0]["IGST_INTRA"].ToString()
                };
                return TranDtls;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from TranDtls " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog("Error from TranDtls " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString());
                throw;
            }
        }

        public static DocDtls GetDocumentDetails(DataTable dt)
        {
            try
            {
                DocDtls DocDtls = new DocDtls
                {
                    Typ = dt.Rows[0]["DOC_TYP"].ToString(),
                    No = dt.Rows[0]["DOC_NO"].ToString(),
                    Dt = Convert.ToDateTime(dt.Rows[0]["DOC_DT"]).ToString("dd/MM/yyyy").Replace("-", "/")
                    //Dt="25/12/2020"
                };
                return DocDtls;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from DocDtls " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog("Error from DocDtls " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString());
                throw;
            }
        }

        public static SellerDtls GetSellerDetails(DataTable dt)
        {
            try
            {
                SellerDtls SellerDtls = new SellerDtls
                {
                    Gstin = dt.Rows[0]["BILLFROM_GSTIN"].ToString(),        //Mandatory
                    LglNm = dt.Rows[0]["BILLFROM_TRDNM"].ToString(),       //Mandatory
                    TrdNm = dt.Rows[0]["BILLFROM_TRDNM"].ToString() == "" ? null : dt.Rows[0]["BILLFROM_TRDNM"].ToString(),
                    Addr1 = dt.Rows[0]["BILLFROM_BNO"].ToString().Trim(),     //Mandatory
                    //Addr2 = dt.Rows[0]["BILLFROM_BNO"].ToString() + " " + dt.Rows[0]["BILLFROM_BNM"].ToString() + " " + dt.Rows[0]["BILLFROM_FLNO"].ToString() + " " + dt.Rows[0]["BILLFROM_DST"].ToString(),
                    Addr2 = null,

                    Loc = dt.Rows[0]["BILLFROM_LOC"].ToString(),                                                    //Mandatory
                    Pin = Convert.ToInt32(dt.Rows[0]["BILLFROM_PIN"].ToString() == "" ? "201301" : dt.Rows[0]["BILLFROM_PIN"].ToString()),
                    Stcd = dt.Rows[0]["BILLFROM_STCD"].ToString() == "" ? "0" : dt.Rows[0]["BILLFROM_STCD"].ToString(),
                    //Ph = dt.Rows[0]["BILLFROM_PH"].ToString() == "" ? null : dt.Rows[0]["BILLFROM_PH"].ToString(),
                    //Em = dt.Rows[0]["BILLFROM_EM"].ToString() == "" ? null : dt.Rows[0]["BILLFROM_EM"].ToString()
                    Ph = null,
                    Em = null
                };
                return SellerDtls;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from SellerDtls " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog("Error from SellerDtls " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString());
                throw;
            }
        }

        public static BuyerDtls GetBuyerDetails(DataTable dt)
        {
            try
            {
                BuyerDtls BuyerDtls = new BuyerDtls
                {
                    Gstin = dt.Rows[0]["BILLTO_GSTIN"].ToString(),
                    LglNm = dt.Rows[0]["BILLTO_TRDNM"].ToString(),
                    TrdNm = dt.Rows[0]["BILLTO_TRDNM"].ToString() == "" ? null : dt.Rows[0]["BILLTO_TRDNM"].ToString(),
                    Pos = dt.Rows[0]["place_of_supply"].ToString() == "" ? null : dt.Rows[0]["place_of_supply"].ToString(),
                    Addr1 = dt.Rows[0]["BILLTO_BNO"].ToString().Trim(),     //Mandatory
                    //Addr2 = dt.Rows[0]["BILLTO_BNO"].ToString() + " " + dt.Rows[0]["BILLTO_BNM"].ToString() + " " + dt.Rows[0]["BILLTO_FLNO"].ToString() + " " + dt.Rows[0]["BILLTO_DST"].ToString(),
                    Addr2 = null,
                    Loc = dt.Rows[0]["BILLTO_LOC"].ToString() == "" ? "" : dt.Rows[0]["BILLTO_LOC"].ToString(),
                    Pin = Convert.ToInt32(dt.Rows[0]["BILLTO_PIN"].ToString() == "" ? "201301" : dt.Rows[0]["BILLTO_PIN"].ToString()),
                    Stcd = dt.Rows[0]["BILLTO_STCD"].ToString() == "" ? null : dt.Rows[0]["BILLTO_STCD"].ToString(),
                    //Ph = dt.Rows[0]["BILLTO_PH"].ToString() == "" ? null : dt.Rows[0]["BILLTO_PH"].ToString(),
                    //Em = dt.Rows[0]["BILLTO_EM"].ToString() == "" ? null : dt.Rows[0]["BILLTO_EM"].ToString()
                    Ph = null,
                    Em = null
                };
                return BuyerDtls;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from BuyerDtls " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog("Error from BuyerDtls " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString());
                throw;
            }
        }

        public static DispDtls getdispatch_details(DataTable dt)
        {
            try
            {
                DispDtls dispatch_details = new DispDtls
                {
                    Nm = dt.Rows[0]["SHIPFROM_TRDNM"].ToString() == "" ? "" : dt.Rows[0]["SHIPFROM_TRDNM"].ToString(),              //Mandatory
                    //Addr1 = dt.Rows[0]["SHIPFROM_BNO"].ToString().Trim(),     //Mandatory
                    Addr1 = dt.Rows[0]["SHIPFROM_BNO"].ToString() + " " + dt.Rows[0]["SHIPFROM_BNM"].ToString(),
                    Addr2 = null,
                    Loc = dt.Rows[0]["SHIPFROM_LOC"].ToString() == "" ? "" : dt.Rows[0]["SHIPFROM_LOC"].ToString(),                                       //Mandatory
                    Pin = Convert.ToInt32(dt.Rows[0]["SHIPFROM_PIN"].ToString() == "" ? "201301" : dt.Rows[0]["SHIPFROM_PIN"].ToString()),    //Mandatory
                    Stcd = dt.Rows[0]["SHIPFROM_STCD"].ToString() == "" ? "" : dt.Rows[0]["SHIPFROM_STCD"].ToString()                                        //Mandatory
                };

                return dispatch_details;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from dispatch_details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog("Error from dispatch_details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString());
                throw;
            }
        }

        public static ShipDtls getship_details(DataTable dt)
        {
            try
            {
                ShipDtls ship_details = new ShipDtls
                {
                    Gstin = dt.Rows[0]["SHIPTO_GSTIN"].ToString() == "" ? null : dt.Rows[0]["SHIPTO_GSTIN"].ToString(),
                    LglNm = dt.Rows[0]["SHIPTO_TRDNM"].ToString(),            //Mandatory
                    TrdNm = dt.Rows[0]["SHIPTO_TRDNM"].ToString() == "" ? null : dt.Rows[0]["SHIPTO_TRDNM"].ToString(),
                    Addr1 = dt.Rows[0]["SHIPTO_BNO"].ToString().Trim()+"" + dt.Rows[0]["SHIPTO_BNM"].ToString(),         //Mandatory                                                                                                                                                                                       
                    //Addr2 = dt.Rows[0]["SHIPTO_BNO"].ToString() + " " + dt.Rows[0]["SHIPTO_BNM"].ToString() + " " + dt.Rows[0]["SHIPTO_FLNO"].ToString() + " " + dt.Rows[0]["SHIPTO_DST"].ToString(),
                    Addr2 = null,
                    Loc = dt.Rows[0]["SHIPTO_LOC"].ToString(),                                       //Mandatory
                    Pin = Convert.ToInt32(dt.Rows[0]["SHIPTO_PIN"].ToString() == "" ? "201301" : dt.Rows[0]["SHIPTO_PIN"].ToString()),    //Mandatory
                    Stcd = dt.Rows[0]["SHIPTO_STCD"].ToString()                                    //Mandatory
                };

                return ship_details;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from ship_details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog("Error from ship_details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString());
                throw;
            }
        }

        public static List<ItemList> getitem_list(DataTable dt)
        {
            try
            {
                List<ItemList> itm = new List<ItemList>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    itm.Add(new ItemList
                    {
                        SlNo = Convert.ToString(i + 1),                                                                                      //Mandatory
                        PrdDesc = dt.Rows[i]["ITEM_PRDDESC"].ToString(),
                        IsServc = dt.Rows[i]["ITEM_IS_SERVICE"].ToString() == "" ? "" : dt.Rows[i]["ITEM_IS_SERVICE"].ToString(),            //Mandatory
                        HsnCd = dt.Rows[i]["ITEM_HSNCD"].ToString() == "" ? "" : dt.Rows[i]["ITEM_HSNCD"].ToString(),                        //Mandatory
                        Barcde = dt.Rows[i]["ITEM_BARCDE"].ToString() == "" ? null : dt.Rows[i]["ITEM_BARCDE"].ToString(),
                        Qty = Convert.ToDouble(dt.Rows[i]["ITEM_QTY"].ToString()),
                        FreeQty = Convert.ToInt32(dt.Rows[i]["ITEM_FREEQTY"].ToString()),
                        Unit = dt.Rows[i]["ITEM_UNIT"].ToString() == "" ? "" : dt.Rows[i]["ITEM_UNIT"].ToString(),
                        UnitPrice = Convert.ToDouble(dt.Rows[i]["ITEM_UNITPRICE"].ToString()),                                                 //Mandatory
                        TotAmt = Convert.ToDouble(dt.Rows[i]["ITEM_TOTAMT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_TOTAMT"].ToString()),    //Mandatory   

                        Discount = Convert.ToDouble(dt.Rows[i]["ITEM_DISCOUNT"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_DISCOUNT"].ToString()),
                        PreTaxVal = Convert.ToDouble(dt.Rows[i]["ITEM_PRETAX_VALUE"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_PRETAX_VALUE"].ToString()),
                        AssAmt = Convert.ToDouble(dt.Rows[i]["ITEM_ASSAMT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_ASSAMT"].ToString()),       //Mandatory
                        GstRt = Convert.ToDouble(dt.Rows[i]["ITEM_SGSTRT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_SGSTRT"].ToString()),               //Mandatory

                        IgstAmt = Convert.ToDouble(dt.Rows[i]["ITEM_IGSTAMT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_IGSTAMT"].ToString()),
                        CgstAmt = Convert.ToDouble(dt.Rows[i]["ITEM_CGSTAMT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_CGSTAMT"].ToString()),
                        SgstAmt = Convert.ToDouble(dt.Rows[i]["ITEM_SGSTAMT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_SGSTAMT"].ToString()),
                        CesRt = Convert.ToDouble(dt.Rows[i]["ITEM_CESRT"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_CESRT"].ToString()),
                        CesAmt = Convert.ToDouble(dt.Rows[i]["ITEM_CESAMT"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_CESAMT"].ToString()),
                        CesNonAdvlAmt = Convert.ToDouble(dt.Rows[i]["ITEM_CESNONADVAL"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_CESNONADVAL"].ToString()),
                        StateCesRt = Convert.ToDouble(dt.Rows[i]["ITEM_STATECES"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_STATECES"].ToString()),
                        StateCesAmt = Convert.ToDouble(dt.Rows[i]["ITEM_STATECESAMT"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_STATECESAMT"].ToString()),
                        StateCesNonAdvlAmt = Convert.ToDouble(dt.Rows[i]["ITEM_STATECESNODAMT"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_STATECESNODAMT"].ToString()),
                        OthChrg = Convert.ToDouble(dt.Rows[i]["ITEM_OTHCHRG"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_OTHCHRG"].ToString()),
                        TotItemVal = Convert.ToDouble(dt.Rows[i]["ITEM_TOTITEMVAL"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_TOTITEMVAL"].ToString()),       //Mandatory
                        OrgCntry = "IN",
                        OrdLineRef = "1",
                        PrdSlNo = "101",
                        BchDtls = Get_Batch_details(dt.Rows[i]["ITEM_BATCH_NM"].ToString(), dt.Rows[0]["ITEM_BATCH_EXPDT"].ToString(), dt.Rows[0]["ITEM_BATCH_WRDT"].ToString()),
                        AttribDtls = Get_attribute_details(dt),
                    });
                }
                return itm;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from Item Details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog("Error from Item Details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString());
                throw;
            }

        }

        public static BchDtls Get_Batch_details(string Name, string Expirydt, string WrDate)
        {
            try
            {
                BchDtls batch_details = new BchDtls
                {
                    Nm = Name == "" ? "Testing" : Name,                  //Mandatory
                    Expdt = Expirydt == "" ? null : Convert.ToDateTime(Expirydt).ToString("dd/MM/yyyy"),
                    wrDt = WrDate == "" ? null : Convert.ToDateTime(WrDate).ToString("dd/MM/yyyy"),
                };

                return batch_details;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from batch_details " + ex.StackTrace.ToString());
                throw;
            }
        }

        public static List<AttribDtl> Get_attribute_details(DataTable dt)
        {
            try
            {
                List<AttribDtl> lstattrdtl = new List<AttribDtl>();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                lstattrdtl.Add(new AttribDtl
                {
                    Nm = dt.Rows[0]["ITEM_ATTRIBUTE_DETAILS"].ToString() == "" ? null : dt.Rows[0]["ITEM_ATTRIBUTE_DETAILS"].ToString(),                 //Mandatory
                    Val = dt.Rows[0]["ITEM_ATTRIBUTE_VALUE"].ToString() == "" ? null : dt.Rows[0]["ITEM_ATTRIBUTE_VALUE"].ToString()
                });
                //}
                return lstattrdtl;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static ValDtls getvalue_details(DataTable dt)
        {
            try
            {
                ValDtls value_details = new ValDtls
                {
                    AssVal = Convert.ToDouble(dt.Rows[0]["VAL_ASSVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_ASSVAL"].ToString()),                   //Required
                    CgstVal = Convert.ToDouble(dt.Rows[0]["VAL_CGSTVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_CGSTVAL"].ToString()),
                    SgstVal = Convert.ToDouble(dt.Rows[0]["VAL_SGSTVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_SGSTVAL"].ToString()),
                    IgstVal = Convert.ToDouble(dt.Rows[0]["VAL_IGSTVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_IGSTVAL"].ToString()),
                    CesVal = Convert.ToDouble(dt.Rows[0]["VAL_CESVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_CESVAL"].ToString()),
                    StCesVal = Convert.ToDouble(dt.Rows[0]["VAL_STCESVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_STCESVAL"].ToString()),
                    //total_cess_nonadvol_value = Convert.ToDouble(dt.Rows[0]["VAL_CESNONADVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_CESNONADVAL"].ToString()),
                    Discount = Convert.ToDouble(dt.Rows[0]["VAL_TOTDISCOUNT"].ToString() == "" ? "0" : dt.Rows[0]["VAL_TOTDISCOUNT"].ToString()),
                    OthChrg = Convert.ToDouble(dt.Rows[0]["VAL_OTHER_CHARGE"].ToString() == "" ? "0" : dt.Rows[0]["VAL_OTHER_CHARGE"].ToString()),
                    TotInvVal = Convert.ToDouble(dt.Rows[0]["VAL_TOTINVVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_TOTINVVAL"].ToString()),                //Required
                    RndOffAmt = Convert.ToDouble(dt.Rows[0]["VAL_OTHCHRG"].ToString() == "" ? "0" : dt.Rows[0]["VAL_OTHCHRG"].ToString()),
                    TotInvValFc = Convert.ToDouble(dt.Rows[0]["VAL_TOTINV_ADDCUR"].ToString() == "" ? "0" : dt.Rows[0]["VAL_TOTINV_ADDCUR"].ToString()),
                };
                return value_details;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from value_details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog("Error from value_details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString());
                throw;
            }
        }

        public static PayDtls getpayment_details(DataTable dt)
        {
            try
            {
                PayDtls payment_details = new PayDtls
                {
                    Nm = dt.Rows[0]["PAY_NAM"].ToString() == "" ? null : dt.Rows[0]["PAY_NAM"].ToString(),
                    Accdet = dt.Rows[0]["PAY_ACCTDET"].ToString() == "" ? null : dt.Rows[0]["PAY_ACCTDET"].ToString(),
                    Mode = dt.Rows[0]["PAY_MODE"].ToString() == "" ? null : dt.Rows[0]["PAY_MODE"].ToString(),
                    Fininsbr = dt.Rows[0]["PAY_FININSBR"].ToString() == "" ? null : dt.Rows[0]["PAY_FININSBR"].ToString(),
                    Payterm = dt.Rows[0]["PAY_PAYTERM"].ToString() == "" ? null : dt.Rows[0]["PAY_PAYTERM"].ToString(),
                    Payinstr = dt.Rows[0]["PAY_PAYINSTR"].ToString() == "" ? null : dt.Rows[0]["PAY_PAYINSTR"].ToString(),
                    Crtrn = dt.Rows[0]["PAY_CRTRN"].ToString() == "" ? null : dt.Rows[0]["PAY_CRTRN"].ToString(),
                    Dirdr = dt.Rows[0]["PAY_DIRDR"].ToString() == "" ? null : dt.Rows[0]["PAY_DIRDR"].ToString(),
                    Crday = Convert.ToInt32(dt.Rows[0]["PAY_CRDAY"].ToString() == "" ? null : dt.Rows[0]["PAY_CRDAY"].ToString()),
                    Paidamt = Convert.ToDouble(dt.Rows[0]["PAY_BALAMT"].ToString() == "" ? null : dt.Rows[0]["PAY_BALAMT"].ToString()),
                    Paymtdue = Convert.ToDouble(dt.Rows[0]["PAY_OUTSTANDINGAMT"].ToString() == "" ? null : dt.Rows[0]["PAY_OUTSTANDINGAMT"].ToString())
                };
                return payment_details;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from payment_details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog("Error from payment_details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString());
                throw;
            }
        }

        public static RefDtls getreference_details(DataTable dt)
        {
            try
            {
                RefDtls reference_details = new RefDtls
                {
                    InvRm = dt.Rows[0]["REF_INVRMK"].ToString() == "" ? null : dt.Rows[0]["REF_INVRMK"].ToString(),
                    DocPerdDtls = new DocPerdDtls()
                    {
                        InvStDt = dt.Rows[0]["REF_INVSTDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["REF_INVSTDT"]).ToString("dd/MM/yyyy"),     //Mandatory
                        InvEndDt = dt.Rows[0]["REF_INVENDDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["REF_INVENDDT"]).ToString("dd/MM/yyyy"),     //Mandatory
                    },
                    PrecDocDtls = Get_preceding_document_details(dt),
                    ContrDtls = Get_ContractDetail(dt),

                };
                return reference_details;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<PrecDocDtl> Get_preceding_document_details(DataTable dt)
        {
            List<PrecDocDtl> lstpredocdtl = new List<PrecDocDtl>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            lstpredocdtl.Add(new PrecDocDtl
            {
                InvNo = dt.Rows[0]["REF_INVNO"].ToString(),     //Mandatory
                InvDt = dt.Rows[0]["REF_PRECINVDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["REF_PRECINVDT"]).ToString("dd/MM/yyyy"),     //Mandatory
                OthRefNo = dt.Rows[0]["REF_EXTREF"].ToString() == "" ? null : dt.Rows[0]["REF_EXTREF"].ToString()
            });
            //}
            return lstpredocdtl;
        }

        public static List<ContrDtl> Get_ContractDetail(DataTable dt)
        {
            List<ContrDtl> lstcontractdtl = new List<ContrDtl>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
                lstcontractdtl.Add(new ContrDtl
                {
                    RecAdvRefr = dt.Rows[0]["REF_PRECINVNO"].ToString() == "" ? null : dt.Rows[0]["REF_PRECINVNO"].ToString(),
                    RecAdvDt = dt.Rows[0]["REF_PRECINVDT"].ToString() == "" ? null : Convert.ToDateTime(dt.Rows[0]["REF_PRECINVDT"]).ToString("dd/MM/yyyy"),
                    Tendrefr = dt.Rows[0]["REF_PROJREF"].ToString() == "" ? null : dt.Rows[0]["REF_PROJREF"].ToString(),
                    Contrrefr = dt.Rows[0]["REF_CONTRREF"].ToString() == "" ? null : dt.Rows[0]["REF_CONTRREF"].ToString(),
                    Extrefr = dt.Rows[0]["REF_EXTREF"].ToString() == "" ? null : dt.Rows[0]["REF_EXTREF"].ToString(),
                    Projrefr = dt.Rows[0]["REF_PROJREF"].ToString() == "" ? null : dt.Rows[0]["REF_PROJREF"].ToString(),
                    Porefr = dt.Rows[0]["REF_POREF"].ToString() == "" ? null : dt.Rows[0]["REF_POREF"].ToString(),
                    PoRefDt = dt.Rows[0]["VENDOR_PO_REFDT"].ToString() == "" ? null : Convert.ToDateTime(dt.Rows[0]["VENDOR_PO_REFDT"]).ToString("dd/MM/yyyy")

                });
            //}
            return lstcontractdtl;
        }

        public static List<AddlDocDtl> get_additional_document(DataTable dt)
        {
            List<AddlDocDtl> lstAddDocDtl = new List<AddlDocDtl>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
                lstAddDocDtl.Add(new AddlDocDtl
                {
                    Url = dt.Rows[0]["ADD_DOC_URL"].ToString() == "" ? null : dt.Rows[0]["ADD_DOC_URL"].ToString(),
                    Docs = dt.Rows[0]["ADD_SUPPORTING_DOC"].ToString()==""?null: dt.Rows[0]["ADD_SUPPORTING_DOC"].ToString(),
                    Info = dt.Rows[0]["ADD_ADDITIONAL_INFO"].ToString()==""?null: dt.Rows[0]["ADD_ADDITIONAL_INFO"].ToString()
                });
            //}
            return lstAddDocDtl;
        }

        public static ExpDtls getexport_details(DataTable dt)
        {
            try
            {
                ExpDtls export_details = new ExpDtls
                {
                    ShipBNo = dt.Rows[0]["EXP_SHIPBNO"].ToString() == "" ? null : dt.Rows[0]["EXP_SHIPBNO"].ToString(),
                    ShipBDt = dt.Rows[0]["EXP_SHIPBDT"].ToString() == "" ? null : Convert.ToDateTime(dt.Rows[0]["EXP_SHIPBDT"]).ToString("dd/MM/yyyy"),
                    CntCode = dt.Rows[0]["EXP_CNTCODE"].ToString() == "" ? null : dt.Rows[0]["EXP_CNTCODE"].ToString(),
                    ForCur = dt.Rows[0]["EXP_FORCUR"].ToString() == "" ? null : dt.Rows[0]["EXP_FORCUR"].ToString(),
                    RefClm = dt.Rows[0]["REFUND_CLAIM"].ToString() == "" ? null : dt.Rows[0]["REFUND_CLAIM"].ToString(),
                    Port = dt.Rows[0]["EXP_PORT"].ToString() == "" ? null : dt.Rows[0]["EXP_PORT"].ToString(),
                    ExpDuty = dt.Rows[0]["EXP_DUTY"].ToString() == "" ? null : dt.Rows[0]["EXP_DUTY"].ToString(),

                };

                return export_details;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from export_details " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                throw;
            }
        }

        public static EwbDtls getewaybill_details(DataTable dt)
        {
            try
            {
                int calcdist = 0;
                if (dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "" || dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "0")
                {
                    if (dt.Rows[0]["TRAN_CATG"].ToString().StartsWith("EXP"))
                    {
                        if (dt.Rows[0]["BILLFROM_PIN"].ToString() == dt.Rows[0]["SHIPTO_PIN"].ToString())
                        {
                            calcdist = 20;
                        }
                        else
                        {
                            string Rdistval = DistanceResponseClass.GoogleDistAPI(Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLFROM_PIN"].ToString())), Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["SHIPTO_PIN"].ToString())));
                            calcdist = Convert.ToInt32(Rdistval);
                            string commandText = "UPDATE einvoice_generate_temp SET EWAY_TRANSPORTAR_DISTANCE='" + calcdist + "' WHERE DOC_NO ='" + dt.Rows[0]["DOC_No"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, commandText);
                        }
                    }
                    else
                    {
                        if (dt.Rows[0]["BILLFROM_PIN"].ToString() == dt.Rows[0]["BILLTO_PIN"].ToString())
                        {
                            calcdist = 20;
                        }
                        else
                        {
                            string Rdistval = DistanceResponseClass.GoogleDistAPI(Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLFROM_PIN"].ToString())), Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLTO_PIN"].ToString())));
                            calcdist = Convert.ToInt32(Rdistval);
                            string commandText = "UPDATE einvoice_generate_temp SET EWAY_TRANSPORTAR_DISTANCE='" + calcdist + "' WHERE DOC_NO ='" + dt.Rows[0]["DOC_No"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, commandText);
                        }
                    }
                }

                EwbDtls EwaybillDetails = new EwbDtls
                {
                    Transid = dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString(),
                    Transname = dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString(),
                    TransMode = dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString(),                  //Mandatory
                    Distance = (dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "" || dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString() == "0") ? calcdist : Convert.ToInt32(dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString()),     //Mandatory
                    Transdocno = dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString(),
                    TransdocDt = dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"].ToString() == "" ? null : Convert.ToDateTime(dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"]).ToString("dd/MM/yyyy").Replace("-", "/"),
                    Vehno = dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString(),
                    Vehtype = dt.Rows[0]["EWAY_TRANSPORTAR_VEHITYPE"].ToString() == "" ? null : dt.Rows[0]["EWAY_TRANSPORTAR_VEHITYPE"].ToString()
                };
                return EwaybillDetails;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog("Error from EwaybillDetails " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog("Error from EwaybillDetails " + ex.StackTrace.ToString(), dt.Rows[0]["DOC_No"].ToString());
                throw;
            }
        }

    }

    public class TranDtls
    {
        public string TaxSch { get; set; }
        public string SupTyp { get; set; }
        public string RegRev { get; set; }
        public object EcmGstin { get; set; }
        public string IgstOnIntra { get; set; }
    }

    public class DocDtls
    {
        public string Typ { get; set; }
        public string No { get; set; }
        public string Dt { get; set; }
    }

    public class SellerDtls
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
        public string Ph { get; set; }
        public string Em { get; set; }
    }

    public class BuyerDtls
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Pos { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
        public string Ph { get; set; }
        public string Em { get; set; }
    }

    public class DispDtls
    {
        public string Nm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
    }

    public class ShipDtls
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
    }

    public class BchDtls
    {
        public string Nm { get; set; }
        public string Expdt { get; set; }
        public string wrDt { get; set; }
    }

    public class AttribDtl
    {
        public string Nm { get; set; }
        public string Val { get; set; }
    }



    public class ValDtls
    {
        public double AssVal { get; set; }
        public double CgstVal { get; set; }
        public double SgstVal { get; set; }
        public double IgstVal { get; set; }
        public double CesVal { get; set; }
        public double StCesVal { get; set; }
        public double Discount { get; set; }
        public double OthChrg { get; set; }
        public double RndOffAmt { get; set; }
        public double TotInvVal { get; set; }
        public double TotInvValFc { get; set; }
    }

    public class PayDtls
    {
        public string Nm { get; set; }
        public string Accdet { get; set; }
        public string Mode { get; set; }
        public string Fininsbr { get; set; }
        public string Payterm { get; set; }
        public string Payinstr { get; set; }
        public string Crtrn { get; set; }
        public string Dirdr { get; set; }
        public int Crday { get; set; }
        public double Paidamt { get; set; }
        public double Paymtdue { get; set; }
    }

    public class RefDtls
    {
        public string InvRm { get; set; }
        public DocPerdDtls DocPerdDtls { get; set; }
        public List<PrecDocDtl> PrecDocDtls { get; set; }
        public List<ContrDtl> ContrDtls { get; set; }
    }

    public class ContrDtl
    {
        public string RecAdvRefr { get; set; }
        public string RecAdvDt { get; set; }
        public string Tendrefr { get; set; }
        public string Contrrefr { get; set; }
        public string Extrefr { get; set; }
        public string Projrefr { get; set; }
        public string Porefr { get; set; }
        public string PoRefDt { get; set; }
    }

    public class DocPerdDtls
    {
        public string InvStDt { get; set; }
        public string InvEndDt { get; set; }
    }

    public class PrecDocDtl
    {
        public string InvNo { get; set; }
        public string InvDt { get; set; }
        public string OthRefNo { get; set; }
    }

    public class AddlDocDtl
    {
        public string Url { get; set; }
        public string Docs { get; set; }
        public string Info { get; set; }
    }

    public class ExpDtls
    {
        public string ShipBNo { get; set; }
        public string ShipBDt { get; set; }
        public string Port { get; set; }
        public string RefClm { get; set; }
        public string ForCur { get; set; }
        public string CntCode { get; set; }
        public object ExpDuty { get; set; }
    }

    public class EwbDtls
    {
        public string Transid { get; set; }
        public string Transname { get; set; }
        public int Distance { get; set; }
        public string Transdocno { get; set; }
        public string TransdocDt { get; set; }
        public string Vehno { get; set; }
        public string Vehtype { get; set; }
        public string TransMode { get; set; }
    }

    public class ItemList
    {
        public string SlNo { get; set; }
        public string PrdDesc { get; set; }
        public string IsServc { get; set; }
        public string HsnCd { get; set; }
        public string Barcde { get; set; }
        public double Qty { get; set; }
        public int FreeQty { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
        public double TotAmt { get; set; }
        public double Discount { get; set; }
        public double PreTaxVal { get; set; }
        public double AssAmt { get; set; }
        public double GstRt { get; set; }
        public double IgstAmt { get; set; }
        public double CgstAmt { get; set; }
        public double SgstAmt { get; set; }
        public double CesRt { get; set; }
        public double CesAmt { get; set; }
        public double CesNonAdvlAmt { get; set; }
        public double StateCesRt { get; set; }
        public double StateCesAmt { get; set; }
        public double StateCesNonAdvlAmt { get; set; }
        public double OthChrg { get; set; }
        public double TotItemVal { get; set; }
        public string OrdLineRef { get; set; }
        public string OrgCntry { get; set; }
        public string PrdSlNo { get; set; }
        public BchDtls BchDtls { get; set; }
        public List<AttribDtl> AttribDtls { get; set; }
    }

    public class GenIrnRoot
    {
        public string Version { get; set; }
        public TranDtls TranDtls { get; set; }
        public DocDtls DocDtls { get; set; }
        public SellerDtls SellerDtls { get; set; }
        public BuyerDtls BuyerDtls { get; set; }
        public DispDtls DispDtls { get; set; }
        public ShipDtls ShipDtls { get; set; }
        public List<ItemList> ItemList { get; set; }
        public ValDtls ValDtls { get; set; }
        public PayDtls PayDtls { get; set; }
        public RefDtls RefDtls { get; set; }
        public List<AddlDocDtl> AddlDocDtls { get; set; }
        public ExpDtls ExpDtls { get; set; }
        public EwbDtls EwbDtls { get; set; }
    }


    public class successResult
    {
        public long AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public string Status { get; set; }
        public string EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
        public object Remarks { get; set; }
    }

    public class Desc
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class SuccessInfo
    {
        public string InfCd { get; set; }
        //public List<Desc> Desc { get; set; }
        public string Desc { get; set; }
    }

    public class SuccessInfo2
    {
        public string InfCd { get; set; }
        public List<Desc> Desc { get; set; }
    }
    public class successRoot
    {
        public bool success { get; set; }
        public string message { get; set; }
        public successResult result { get; set; }
        public List<SuccessInfo> info { get; set; }
    }

    public class successRoot2
    {
        public bool success { get; set; }
        public string message { get; set; }
        public successResult result { get; set; }
        public List<SuccessInfo2> info { get; set; }
    }

    public class DublicateDesc
    {
        public long AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }
    }

    public class DublicateResult
    {
        public string InfCd { get; set; }
        public DublicateDesc Desc { get; set; }
    }

    public class DublicateRoot
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<DublicateResult> result { get; set; }
    }

}

