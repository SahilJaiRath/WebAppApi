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
    public class GenerateEinvoice
    {
        public static string GenerateJsonFile(DataTable dt, string access_token)
        {
            string msg = "";
            //  string mystr = "";
            try
            {
                List<RootObject> customers = new List<RootObject>();

                if (dt.Rows[0]["EINV_WITH_EWB"].ToString() == "Y")
                {
                    RootObject TranDtls = new RootObject
                    {
                        access_token = access_token,
                        user_gstin = dt.Rows[0]["GSTIN"].ToString(),
                        data_source = "erp",
                        transaction_details = Gettransaction_details(dt),
                        document_details = getdocument_details(dt),
                        seller_details = getseller_details(dt),
                        buyer_details = getbuyer_details(dt),

                        //********************Not Required For einvoice*******************8

                        dispatch_details = getdispatch_details(dt),
                        ship_details = getship_details(dt),
                        export_details = getexport_details(dt),
                        payment_details = getpayment_details(dt),
                        reference_details = getreference_details(dt),
                        additional_document_details = get_additional_document(dt),
                        ewaybill_details = getewaybill_details(dt),

                        //********************End*******************8
                        value_details = getvalue_details(dt),
                        item_list = getitem_list(dt),

                    };
                    customers.Add(TranDtls);
                }
                else
                {
                    RootObject TranDtls = new RootObject
                    {
                        access_token = access_token,
                        user_gstin = dt.Rows[0]["GSTIN"].ToString(),
                        data_source = "erp",
                        transaction_details = Gettransaction_details(dt),
                        document_details = getdocument_details(dt),
                        seller_details = getseller_details(dt),
                        buyer_details = getbuyer_details(dt),

                        //********************Not Required For einvoice*******************8

                        // dispatch_details = getdispatch_details(dt),
                        //ship_details = getship_details(dt),
                        // export_details = getexport_details(dt),
                        // payment_details = getpayment_details(dt),
                        // reference_details = getreference_details(dt),
                        // additional_document_details = get_additional_document(dt),
                        // ewaybill_details = getewaybill_details(dt),

                        //********************End*******************8
                        value_details = getvalue_details(dt),
                        item_list = getitem_list(dt),

                    };
                    customers.Add(TranDtls);
                }

                //var json = new JavaScriptSerializer().Serialize(customers);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(customers);
                string mystr = json.Substring(1, json.Length - 2);
                msg = mystr;
                return msg;
            }
            catch (Exception ex)
            {               
                return msg = "0";
            }

        }

        public static async Task<dynamic> generateInvoice(dynamic JsonFile, string sekdec, DataTable dt)
        {
            dynamic InvResult = "", InvResDecData = "";
            HttpResponseMessage response = null;
            try
            {
                //string URL = "https://clientbasic.mastersindia.co/generateEinvoice";
                string URL = All_API_Urls.EinvGenerateUrl;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(JsonFile, Encoding.UTF8, "application/json");
                    response = client.PostAsync(new Uri(URL), content).Result;
                    // response = await client.PostAsync(new Uri(URL), content);
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        var value2 = JsonConvert.DeserializeObject<INVRootObject>(LRData2);
                        if (value2.results.code == 200)
                        {
                            //InvResDecData = value2.results.message.Irn;
                            InvResDecData = "1";
                            INVRootObject oj = value2;
                            //SqlConnectionDB.InsertDataSql(oj, dt);                            
                            OracelDataInsert.UpdateDataOracle(oj, dt);

                            //enabled if you required QR code
                            //All_API_Urls.GenerateQRcodeImage(oj.results.message.Irn, oj.results.message.SignedQRCode, dt.Rows[0][""].ToString());
                        }
                        else
                        {
                            INVRootObject oj = value2;
                            //SqlConnectionDB.InsertDataSql(oj, dt);
                            //OracelDataInsert.InsertDataOracle(oj, dt);
                            OracelDataInsert.UpdateDataOracle(oj, dt);
                            //update insert data 
                        }
                    }
                    else
                    {
                        InvResDecData = response;
                    }
                }
                return InvResDecData;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where id='" + dt.Rows[0]["ID"].ToString() + "' and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
               
                return InvResDecData = ex.Message;
            }

        }
        public static TransactionDetails Gettransaction_details(DataTable dt)
        {
            TransactionDetails transaction_details = new TransactionDetails
            {
                supply_type = dt.Rows[0]["TRAN_CATG"].ToString(),                                               //Mandatory
                charge_type = dt.Rows[0]["TRAN_ECMTRN"].ToString(),
                igst_on_intra = dt.Rows[0]["IGST_INTRA"].ToString(),
                ecommerce_gstin = dt.Rows[0]["ecommerce_gstin"].ToString()
            };
            return transaction_details;
        }

        public static DocumentDetails getdocument_details(DataTable dt)
        {
            DocumentDetails document_details = new DocumentDetails
            {
                //original_document_number = ""

                document_type = dt.Rows[0]["DOC_TYP"].ToString(),                                               //Mandatory
                document_number = dt.Rows[0]["DOC_NO"].ToString(),                                              //Mandatory
                document_date = Convert.ToDateTime(dt.Rows[0]["DOC_DT"]).ToString("dd/MM/yyyy")                //Mandatory
            };
            return document_details;
        }
        public static SellerDetails getseller_details(DataTable dt)
        {
            SellerDetails seller_details = new SellerDetails
            {
                gstin = dt.Rows[0]["BILLFROM_GSTIN"].ToString() == "" ? "" : dt.Rows[0]["BILLFROM_GSTIN"].ToString(),                                         //Mandatory
                legal_name = dt.Rows[0]["BILLFROM_TRDNM"].ToString() == "" ? "" : dt.Rows[0]["BILLFROM_TRDNM"].ToString(),                                   //Mandatory
                trade_name = dt.Rows[0]["BILLFROM_TRDNM"].ToString(),
                address1 = dt.Rows[0]["BILLFROM_BNO"].ToString() + " " + dt.Rows[0]["BILLFROM_BNM"].ToString() + " " + dt.Rows[0]["BILLFROM_FLNO"].ToString() + " " + dt.Rows[0]["BILLFROM_DST"].ToString(),     //Mandatory
                address2 = dt.Rows[0]["BILLFROM_BNO"].ToString() + " " + dt.Rows[0]["BILLFROM_BNM"].ToString() + " " + dt.Rows[0]["BILLFROM_FLNO"].ToString() + " " + dt.Rows[0]["BILLFROM_DST"].ToString(),
                location = dt.Rows[0]["BILLFROM_LOC"].ToString() == "" ? "" : dt.Rows[0]["BILLFROM_LOC"].ToString(),                                            //Mandatory
                pincode = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLFROM_PIN"].ToString() == "" ? "201301" : dt.Rows[0]["BILLFROM_PIN"].ToString())),    //Mandatory
                state_code = dt.Rows[0]["BILLFROM_STCD"].ToString() == "" ? "0" : dt.Rows[0]["BILLFROM_STCD"].ToString(),                                        //Mandatory
                phone_number = dt.Rows[0]["BILLFROM_PH"].ToString(),
                email = dt.Rows[0]["BILLFROM_EM"].ToString() == "" ? "" : dt.Rows[0]["BILLFROM_EM"].ToString()
            };
            return seller_details;
        }
        public static BuyerDetails getbuyer_details(DataTable dt)
        {
            BuyerDetails getbuyer_details = new BuyerDetails
            {
                gstin = dt.Rows[0]["BILLTO_GSTIN"].ToString() == "" ? "09AAAPG7885R002" : dt.Rows[0]["BILLTO_GSTIN"].ToString(),                    //Mandatory
                legal_name = dt.Rows[0]["BILLTO_TRDNM"].ToString() == "" ? "MAWAI INFOTECH LTD.-HO" : dt.Rows[0]["BILLTO_TRDNM"].ToString(),        //Mandatory
                trade_name = dt.Rows[0]["BILLTO_TRDNM"].ToString() == "" ? "MAWAI INFOTECH LTD.-HO" : dt.Rows[0]["BILLTO_TRDNM"].ToString(),
                address1 = dt.Rows[0]["BILLTO_BNO"].ToString() + " " + dt.Rows[0]["BILLTO_BNM"].ToString() + " " + dt.Rows[0]["BILLTO_FLNO"].ToString() + " " + dt.Rows[0]["BILLTO_DST"].ToString(),     //Mandatory
                address2 = dt.Rows[0]["BILLTO_BNO"].ToString() + " " + dt.Rows[0]["BILLTO_BNM"].ToString() + " " + dt.Rows[0]["BILLTO_FLNO"].ToString() + " " + dt.Rows[0]["BILLTO_DST"].ToString(),
                location = dt.Rows[0]["BILLTO_LOC"].ToString() == "" ? "" : dt.Rows[0]["BILLTO_LOC"].ToString(),                                     //Mandatory
                pincode = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["BILLTO_PIN"].ToString() == "" ? "201301" : dt.Rows[0]["BILLTO_PIN"].ToString())),
                place_of_supply = dt.Rows[0]["place_of_supply"].ToString() == "" ? "9" : dt.Rows[0]["place_of_supply"].ToString(),                    //Mandatory
                state_code = dt.Rows[0]["BILLTO_STCD"].ToString() == "" ? "9" : dt.Rows[0]["BILLTO_STCD"].ToString(),
                phone_number = dt.Rows[0]["BILLTO_PH"].ToString(),
                email = dt.Rows[0]["BILLTO_EM"].ToString() == "" ? "" : dt.Rows[0]["BILLTO_EM"].ToString()
            };

            return getbuyer_details;
        }
        public static DispatchDetails getdispatch_details(DataTable dt)
        {
            DispatchDetails dispatch_details = new DispatchDetails
            {
                company_name = dt.Rows[0]["SHIPFROM_TRDNM"].ToString() == "" ? "MAWAI INFOTECH LTD.-HO" : dt.Rows[0]["SHIPFROM_TRDNM"].ToString(),              //Mandatory
                address1 = dt.Rows[0]["SHIPFROM_BNO"].ToString() + " " + dt.Rows[0]["SHIPFROM_BNM"].ToString() + " " + dt.Rows[0]["SHIPFROM_FLNO"].ToString() + " " + dt.Rows[0]["SHIPFROM_DST"].ToString(),     //Mandatory
                address2 = dt.Rows[0]["SHIPFROM_BNO"].ToString() + " " + dt.Rows[0]["SHIPFROM_BNM"].ToString() + " " + dt.Rows[0]["SHIPFROM_FLNO"].ToString() + " " + dt.Rows[0]["SHIPFROM_DST"].ToString(),
                location = dt.Rows[0]["SHIPFROM_LOC"].ToString() == "" ? "NOIDA" : dt.Rows[0]["SHIPFROM_LOC"].ToString(),                                       //Mandatory
                pincode = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["SHIPFROM_PIN"].ToString() == "" ? "201301" : dt.Rows[0]["SHIPFROM_PIN"].ToString())),    //Mandatory
                state_code = dt.Rows[0]["SHIPFROM_STCD"].ToString() == "" ? "UTTAR PRADESH" : dt.Rows[0]["SHIPFROM_STCD"].ToString()                                        //Mandatory
            };

            return dispatch_details;
        }
        public static ShipDetails getship_details(DataTable dt)
        {
            ShipDetails ship_details = new ShipDetails
            {
                gstin = dt.Rows[0]["SHIPTO_GSTIN"].ToString() == "" ? "09AAAPG7885R002" : dt.Rows[0]["SHIPTO_GSTIN"].ToString(),
                legal_name = dt.Rows[0]["SHIPTO_TRDNM"].ToString() == "" ? "MAWAI INFOTECH LTD.-HO" : dt.Rows[0]["SHIPTO_TRDNM"].ToString(),            //Mandatory
                trade_name = dt.Rows[0]["SHIPTO_TRDNM"].ToString() == "" ? "MAWAI INFOTECH LTD.-HO" : dt.Rows[0]["SHIPTO_TRDNM"].ToString(),
                address1 = dt.Rows[0]["SHIPTO_BNO"].ToString() + " " + dt.Rows[0]["SHIPTO_BNM"].ToString() + " " + dt.Rows[0]["SHIPTO_FLNO"].ToString() + " " + dt.Rows[0]["SHIPTO_DST"].ToString(),         //Mandatory
                address2 = dt.Rows[0]["SHIPTO_BNO"].ToString() + " " + dt.Rows[0]["SHIPTO_BNM"].ToString() + " " + dt.Rows[0]["SHIPTO_FLNO"].ToString() + " " + dt.Rows[0]["SHIPTO_DST"].ToString(),
                location = dt.Rows[0]["SHIPTO_LOC"].ToString() == "" ? "NOIDA" : dt.Rows[0]["SHIPTO_LOC"].ToString(),                                       //Mandatory
                pincode = Convert.ToInt32(Convert.ToDouble(dt.Rows[0]["SHIPTO_PIN"].ToString() == "" ? "201301" : dt.Rows[0]["SHIPTO_PIN"].ToString())),    //Mandatory
                state_code = dt.Rows[0]["SHIPTO_STCD"].ToString() == "" ? "9" : dt.Rows[0]["SHIPTO_STCD"].ToString()                                        //Mandatory
            };

            return ship_details;
        }
        public static ExportDetails getexport_details(DataTable dt)
        {
            ExportDetails export_details = new ExportDetails
            {
                ship_bill_number = dt.Rows[0]["EXP_SHIPBNO"].ToString() == "" ? "qwe1233" : dt.Rows[0]["EXP_SHIPBNO"].ToString(),
                ship_bill_date = dt.Rows[0]["EXP_SHIPBDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["EXP_SHIPBDT"]).ToString("dd/MM/yyyy"),
                country_code = dt.Rows[0]["EXP_CNTCODE"].ToString() == "" ? "IN" : dt.Rows[0]["EXP_CNTCODE"].ToString(),
                foreign_currency = dt.Rows[0]["EXP_FORCUR"].ToString() == "" ? "INR" : dt.Rows[0]["EXP_FORCUR"].ToString(),
                refund_claim = dt.Rows[0]["REFUND_CLAIM"].ToString() == "" ? "N" : dt.Rows[0]["REFUND_CLAIM"].ToString(),
                port_code = dt.Rows[0]["EXP_PORT"].ToString() == "" ? "" : dt.Rows[0]["EXP_PORT"].ToString(),
                export_duty = dt.Rows[0]["EXP_DUTY"].ToString() == "" ? "10000.42" : dt.Rows[0]["EXP_DUTY"].ToString(),

            };

            return export_details;
        }
        public static PaymentDetails getpayment_details(DataTable dt)
        {
            PaymentDetails payment_details = new PaymentDetails
            {
                bank_account_number = dt.Rows[0]["PAY_ACCTDET"].ToString() == "" ? "" : dt.Rows[0]["PAY_ACCTDET"].ToString(),
                paid_balance_amount = Convert.ToDouble(dt.Rows[0]["PAY_BALAMT"].ToString() == "" ? "0" : dt.Rows[0]["PAY_BALAMT"].ToString()),
                credit_days = Convert.ToDouble(dt.Rows[0]["PAY_CRDAY"].ToString() == "" ? "0" : dt.Rows[0]["PAY_CRDAY"].ToString()),
                credit_transfer = dt.Rows[0]["PAY_CRTRN"].ToString() == "" ? "" : dt.Rows[0]["PAY_CRTRN"].ToString(),
                direct_debit = dt.Rows[0]["PAY_DIRDR"].ToString() == "" ? "" : dt.Rows[0]["PAY_DIRDR"].ToString(),
                branch_or_ifsc = dt.Rows[0]["PAY_FININSBR"].ToString() == "" ? "" : dt.Rows[0]["PAY_FININSBR"].ToString(),
                payment_mode = dt.Rows[0]["PAY_MODE"].ToString() == "" ? "" : dt.Rows[0]["PAY_MODE"].ToString(),
                payee_name = dt.Rows[0]["PAY_NAM"].ToString() == "" ? "" : dt.Rows[0]["PAY_NAM"].ToString(),
                //payment_due_date = dt.Rows[0]["PAY_PAYDUEDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["PAY_PAYDUEDT"]).ToString("dd/MM/yyyy"),
                payment_instruction = dt.Rows[0]["PAY_PAYINSTR"].ToString() == "" ? "" : dt.Rows[0]["PAY_PAYINSTR"].ToString(),
                payment_term = dt.Rows[0]["PAY_PAYTERM"].ToString() == "" ? "" : dt.Rows[0]["PAY_PAYTERM"].ToString(),
                outstanding_amount = Convert.ToDouble(dt.Rows[0]["PAY_OUTSTANDINGAMT"].ToString() == "" ? "0" : dt.Rows[0]["PAY_OUTSTANDINGAMT"].ToString()),
            };
            return payment_details;
        }
        public static ReferenceDetails getreference_details(DataTable dt)
        {
            ReferenceDetails reference_details = new ReferenceDetails
            {
                invoice_remarks = dt.Rows[0]["REF_INVRMK"].ToString() == "" ? "Testing" : dt.Rows[0]["REF_INVRMK"].ToString(),
                //invoice_period_start_date = dt.Rows[0]["REF_INVSTDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["REF_INVSTDT"]).ToString("dd/MM/yyyy"),     //Mandatory
                //invoice_period_end_date = dt.Rows[0]["REF_INVENDDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["REF_INVENDDT"]).ToString("dd/MM/yyyy"),     //Mandatory
                document_period_details = new DocumentPeriodDetails()
                {
                    invoice_period_start_date = dt.Rows[0]["REF_INVSTDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["REF_INVSTDT"]).ToString("dd/MM/yyyy"),     //Mandatory
                    invoice_period_end_date = dt.Rows[0]["REF_INVENDDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["REF_INVENDDT"]).ToString("dd/MM/yyyy"),     //Mandatory
                },
                preceding_document_details = Get_preceding_document_details(dt),
                contract_details = Get_ContractDetail(dt),

            };
            return reference_details;
        }

        public static List<PrecedingDocumentDetail> Get_preceding_document_details(DataTable dt)
        {
            List<PrecedingDocumentDetail> lstpredocdtl = new List<PrecedingDocumentDetail>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstpredocdtl.Add(new PrecedingDocumentDetail
                {
                    reference_of_original_invoice = dt.Rows[i]["REF_INVNO"].ToString() == "" ? "" : dt.Rows[i]["REF_INVNO"].ToString(),     //Mandatory
                    preceding_invoice_date = dt.Rows[i]["REF_PRECINVDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[i]["REF_PRECINVDT"]).ToString("dd/MM/yyyy"),     //Mandatory
                    other_reference = dt.Rows[i]["REF_EXTREF"].ToString() == "" ? "" : dt.Rows[i]["REF_EXTREF"].ToString()
                });
            }
            return lstpredocdtl;
        }

        public static List<ContractDetail> Get_ContractDetail(DataTable dt)
        {
            List<ContractDetail> lstcontractdtl = new List<ContractDetail>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstcontractdtl.Add(new ContractDetail
                {
                    receipt_advice_number = dt.Rows[i]["REF_PRECINVNO"].ToString() == "" ? "" : dt.Rows[i]["REF_PRECINVNO"].ToString(),
                    receipt_advice_date = dt.Rows[i]["REF_PRECINVDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[i]["REF_PRECINVDT"]).ToString("dd/MM/yyyy"),
                    batch_reference_number = dt.Rows[i]["REF_PROJREF"].ToString() == "" ? "" : dt.Rows[i]["REF_PROJREF"].ToString(),
                    contract_reference_number = dt.Rows[i]["REF_CONTRREF"].ToString() == "" ? "" : dt.Rows[i]["REF_CONTRREF"].ToString(),
                    other_reference = dt.Rows[i]["REF_EXTREF"].ToString() == "" ? "" : dt.Rows[i]["REF_EXTREF"].ToString(),
                    project_reference_number = dt.Rows[i]["REF_PROJREF"].ToString() == "" ? "" : dt.Rows[i]["REF_PROJREF"].ToString(),
                    vendor_po_reference_number = dt.Rows[i]["REF_POREF"].ToString() == "" ? "" : dt.Rows[i]["REF_POREF"].ToString(),
                    vendor_po_reference_date = dt.Rows[i]["VENDOR_PO_REFDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[i]["VENDOR_PO_REFDT"]).ToString("dd/MM/yyyy")

                });
            }
            return lstcontractdtl;
        }

        public static ValueDetails getvalue_details(DataTable dt)
        {
            ValueDetails reference_details = new ValueDetails
            {
                total_assessable_value = Convert.ToDouble(dt.Rows[0]["VAL_ASSVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_ASSVAL"].ToString()),                   //Required
                total_cgst_value = Convert.ToDouble(dt.Rows[0]["VAL_CGSTVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_CGSTVAL"].ToString()),
                total_sgst_value = Convert.ToDouble(dt.Rows[0]["VAL_SGSTVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_SGSTVAL"].ToString()),
                total_igst_value = Convert.ToDouble(dt.Rows[0]["VAL_IGSTVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_IGSTVAL"].ToString()),
                total_cess_value = Convert.ToDouble(dt.Rows[0]["VAL_CESVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_CESVAL"].ToString()),
                total_cess_value_of_state = Convert.ToDouble(dt.Rows[0]["VAL_STCESVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_STCESVAL"].ToString()),
                //total_cess_nonadvol_value = Convert.ToDouble(dt.Rows[0]["VAL_CESNONADVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_CESNONADVAL"].ToString()),
                total_discount = Convert.ToDouble(dt.Rows[0]["VAL_TOTDISCOUNT"].ToString() == "" ? "0" : dt.Rows[0]["VAL_TOTDISCOUNT"].ToString()),
                total_other_charge = Convert.ToDouble(dt.Rows[0]["VAL_OTHER_CHARGE"].ToString() == "" ? "0" : dt.Rows[0]["VAL_OTHER_CHARGE"].ToString()),
                total_invoice_value = Convert.ToDouble(dt.Rows[0]["VAL_TOTINVVAL"].ToString() == "" ? "0" : dt.Rows[0]["VAL_TOTINVVAL"].ToString()),                //Required
                round_off_amount = Convert.ToDouble(dt.Rows[0]["VAL_ROUNDOFF_AMT"].ToString() == "" ? "0" : dt.Rows[0]["VAL_ROUNDOFF_AMT"].ToString()),
                total_invoice_value_additional_currency = Convert.ToDouble(dt.Rows[0]["VAL_TOTINV_ADDCUR"].ToString() == "" ? "0" : dt.Rows[0]["VAL_TOTINV_ADDCUR"].ToString()),
            };
            return reference_details;
        }

        public static List<ItemList> getitem_list(DataTable dt)
        {
            List<ItemList> itm = new List<ItemList>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itm.Add(new ItemList
                {
                    item_serial_number = Convert.ToString(i + 1),                                                                             //Mandatory
                    product_description = dt.Rows[i]["ITEM_PRDDESC"].ToString(),
                    is_service = dt.Rows[i]["ITEM_IS_SERVICE"].ToString() == "" ? "" : dt.Rows[i]["ITEM_IS_SERVICE"].ToString(),            //Mandatory
                    hsn_code = dt.Rows[i]["ITEM_HSNCD"].ToString() == "" ? "" : dt.Rows[i]["ITEM_HSNCD"].ToString(),                        //Mandatory
                    bar_code = dt.Rows[i]["ITEM_BARCDE"].ToString() == "" ? "" : dt.Rows[i]["ITEM_BARCDE"].ToString(),
                    quantity = Convert.ToDouble(dt.Rows[i]["ITEM_QTY"].ToString()),
                    free_quantity = Convert.ToDouble(dt.Rows[i]["ITEM_FREEQTY"].ToString()),
                    unit = dt.Rows[i]["ITEM_UNIT"].ToString() == "" ? "" : dt.Rows[i]["ITEM_UNIT"].ToString(),
                    unit_price = Convert.ToDouble(dt.Rows[i]["ITEM_UNITPRICE"].ToString()),                                                 //Mandatory
                    total_amount = Convert.ToDouble(dt.Rows[i]["ITEM_TOTAMT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_TOTAMT"].ToString()),   //Mandatory
                    pre_tax_value = Convert.ToDouble(dt.Rows[i]["ITEM_PRETAX_VALUE"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_PRETAX_VALUE"].ToString()),
                    discount = Convert.ToDouble(dt.Rows[i]["ITEM_DISCOUNT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_DISCOUNT"].ToString()),
                    other_charge = Convert.ToDouble(dt.Rows[i]["ITEM_OTHCHRG"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_OTHCHRG"].ToString()),
                    assessable_value = Convert.ToDouble(dt.Rows[i]["ITEM_ASSAMT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_ASSAMT"].ToString()),       //Mandatory
                    gst_rate = Convert.ToDouble(dt.Rows[i]["ITEM_SGSTRT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_SGSTRT"].ToString()),               //Mandatory
                    igst_amount = Convert.ToDouble(dt.Rows[i]["ITEM_IGSTAMT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_IGSTAMT"].ToString()),
                    cgst_amount = Convert.ToDouble(dt.Rows[i]["ITEM_CGSTAMT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_CGSTAMT"].ToString()),
                    sgst_amount = Convert.ToDouble(dt.Rows[i]["ITEM_SGSTAMT"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_SGSTAMT"].ToString()),
                    cess_rate = Convert.ToDouble(dt.Rows[i]["ITEM_CESRT"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_CESRT"].ToString()),
                    cess_amount = Convert.ToDouble(dt.Rows[i]["ITEM_CESAMT"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_CESAMT"].ToString()),
                    cess_nonadvol_amount = Convert.ToDouble(dt.Rows[i]["ITEM_CESNONADVAL"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_CESNONADVAL"].ToString()),
                    state_cess_rate = Convert.ToDouble(dt.Rows[i]["ITEM_STATECES"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_STATECES"].ToString()),
                    state_cess_amount = Convert.ToDouble(dt.Rows[i]["ITEM_STATECESAMT"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_STATECESAMT"].ToString()),
                    state_cess_nonadvol_amount = Convert.ToDouble(dt.Rows[i]["ITEM_STATECESNODAMT"].ToString() == "" ? "0.0" : dt.Rows[i]["ITEM_STATECESNODAMT"].ToString()),
                    total_item_value = Convert.ToDouble(dt.Rows[i]["ITEM_TOTITEMVAL"].ToString() == "" ? "0" : dt.Rows[i]["ITEM_TOTITEMVAL"].ToString()),       //Mandatory
                    country_origin = "IN",
                    order_line_reference = "1",
                    product_serial_number = "101",
                    batch_details = new BatchDetails()
                    {
                        name = dt.Rows[i]["ITEM_BATCH_NM"].ToString() == "" ? "Testing" : dt.Rows[i]["ITEM_BATCH_NM"].ToString(),                  //Mandatory
                        expiry_date = dt.Rows[0]["ITEM_BATCH_EXPDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["ITEM_BATCH_EXPDT"]).ToString("dd/MM/yyyy"),
                        warranty_date = dt.Rows[0]["ITEM_BATCH_WRDT"].ToString() == "" ? Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy") : Convert.ToDateTime(dt.Rows[0]["ITEM_BATCH_WRDT"]).ToString("dd/MM/yyyy"),
                    },                   
                     attribute_details = Get_attribute_details(dt),
                                      
                });
            }
            return itm;
        }

        public static List<AttributeDetail> Get_attribute_details(DataTable dt)
        {
            List<AttributeDetail> lstattrdtl = new List<AttributeDetail>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
                lstattrdtl.Add(new AttributeDetail
                {
                    item_attribute_details = dt.Rows[0]["ITEM_ATTRIBUTE_DETAILS"].ToString() == "" ? "Testing" : dt.Rows[0]["ITEM_ATTRIBUTE_DETAILS"].ToString(),                 //Mandatory
                    item_attribute_value = dt.Rows[0]["ITEM_ATTRIBUTE_VALUE"].ToString() == "" ? "1001" : dt.Rows[0]["ITEM_ATTRIBUTE_VALUE"].ToString()
                });
            //}
            return lstattrdtl;
        }

        public static List<AdditionalDocumentDetail> get_additional_document(DataTable dt)
        {
            List<AdditionalDocumentDetail> lstAddDocDtl = new List<AdditionalDocumentDetail>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAddDocDtl.Add(new AdditionalDocumentDetail
                {
                    supporting_document_url = dt.Rows[i]["ADD_DOC_URL"].ToString(),
                    supporting_document = dt.Rows[i]["ADD_SUPPORTING_DOC"].ToString(),
                    additional_information = dt.Rows[i]["ADD_ADDITIONAL_INFO"].ToString(),

                });
            }
            return lstAddDocDtl;
        }

        public static EwaybillDetails getewaybill_details(DataTable dt)
        {
            EwaybillDetails EwaybillDetails = new EwaybillDetails
            {
                transporter_id = dt.Rows[0]["EWAY_TRANSPORTAR_ID"].ToString(),
                transporter_name = dt.Rows[0]["EWAY_TRANSPORTAR_NAME"].ToString(),
                transportation_mode = dt.Rows[0]["EWAY_TRANSPORTAR_MODE"].ToString(),                  //Mandatory
                transportation_distance = dt.Rows[0]["EWAY_TRANSPORTAR_DISTANCE"].ToString(),     //Mandatory
                transporter_document_number = dt.Rows[0]["EWAY_TRANSPORTAR_DOCNO"].ToString(),
                transporter_document_date = dt.Rows[0]["EWAY_TRANSPORTAR_DOCDT"].ToString(),
                vehicle_number = dt.Rows[0]["EWAY_TRANSPORTAR_VEHINO"].ToString(),
                vehicle_type = dt.Rows[0]["EWAY_TRANSPORTAR_VEHITYPE"].ToString()
            };
            return EwaybillDetails;
        }
    }

    public class TransactionDetails
    {
        public string supply_type { get; set; }
        public string charge_type { get; set; }

        public string igst_on_intra { get; set; }
        public string ecommerce_gstin { get; set; }
    }

    public class DocumentDetails
    {
        public string document_type { get; set; }
        public string document_number { get; set; }
        public string document_date { get; set; }
    }

    public class SellerDetails
    {
        public string gstin { get; set; }
        public string legal_name { get; set; }
        public string trade_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string location { get; set; }
        public int pincode { get; set; }
        public string state_code { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
    }

    public class BuyerDetails
    {
        public string gstin { get; set; }
        public string legal_name { get; set; }
        public string trade_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string location { get; set; }
        public int pincode { get; set; }
        public string place_of_supply { get; set; }
        public string state_code { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
    }

    public class DispatchDetails
    {
        public string company_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string location { get; set; }
        public int pincode { get; set; }
        public string state_code { get; set; }
    }

    public class ShipDetails
    {
        public string gstin { get; set; }
        public string legal_name { get; set; }
        public string trade_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string location { get; set; }
        public int pincode { get; set; }
        public string state_code { get; set; }
    }

    public class ExportDetails
    {
        public string ship_bill_number { get; set; }
        public string ship_bill_date { get; set; }
        public string country_code { get; set; }
        public string foreign_currency { get; set; }
        public string refund_claim { get; set; }
        public string port_code { get; set; }

        public string export_duty { get; set; }
    }
    public class PaymentDetails
    {
        public string bank_account_number { get; set; }
        public double paid_balance_amount { get; set; }
        public double credit_days { get; set; }
        public string credit_transfer { get; set; }
        public string direct_debit { get; set; }
        public string branch_or_ifsc { get; set; }
        public string payment_mode { get; set; }
        public string payee_name { get; set; }
        //public string payment_due_date { get; set; }
        public string payment_instruction { get; set; }
        public string payment_term { get; set; }

        public double outstanding_amount { get; set; }
    }

    public class DocumentPeriodDetails
    {
        public string invoice_period_start_date { get; set; }
        public string invoice_period_end_date { get; set; }
    }

    public class PrecedingDocumentDetail
    {
        public string reference_of_original_invoice { get; set; }
        public string preceding_invoice_date { get; set; }
        public string other_reference { get; set; }
    }

    public class ContractDetail
    {
        public string receipt_advice_number { get; set; }
        public string receipt_advice_date { get; set; }
        public string batch_reference_number { get; set; }
        public string contract_reference_number { get; set; }
        public string other_reference { get; set; }
        public string project_reference_number { get; set; }
        public string vendor_po_reference_number { get; set; }
        public string vendor_po_reference_date { get; set; }
    }
    public class ReferenceDetails
    {
        public string invoice_remarks { get; set; }
        public DocumentPeriodDetails document_period_details { get; set; }
        public List<PrecedingDocumentDetail> preceding_document_details { get; set; }
        public List<ContractDetail> contract_details { get; set; }
    }

    public class AdditionalDocumentDetail
    {
        public string supporting_document_url { get; set; }
        public string supporting_document { get; set; }
        public string additional_information { get; set; }
    }
    public class ValueDetails
    {
        public double total_assessable_value { get; set; }
        public double total_cgst_value { get; set; }
        public double total_sgst_value { get; set; }
        public double total_igst_value { get; set; }
        public double total_cess_value { get; set; }
        public double total_cess_value_of_state { get; set; }
        public double total_discount { get; set; }
        public double total_other_charge { get; set; }
        public double total_invoice_value { get; set; }
        public double round_off_amount { get; set; }
        public double total_invoice_value_additional_currency { get; set; }
    }


    public class EwaybillDetails
    {
        public string transporter_id { get; set; }
        public string transporter_name { get; set; }
        public string transportation_mode { get; set; }
        public string transportation_distance { get; set; }
        public string transporter_document_number { get; set; }
        public string transporter_document_date { get; set; }
        public string vehicle_number { get; set; }
        public string vehicle_type { get; set; }
    }
    public class BatchDetails
    {
        public string name { get; set; }
        public string expiry_date { get; set; }
        public string warranty_date { get; set; }
    }

    public class AttributeDetail
    {
        public string item_attribute_details { get; set; }
        public string item_attribute_value { get; set; }
    }
    public class ItemList
    {
        public string item_serial_number { get; set; }
        public string product_description { get; set; }
        public string is_service { get; set; }
        public string hsn_code { get; set; }
        public string bar_code { get; set; }
        public double quantity { get; set; }
        public double free_quantity { get; set; }
        public string unit { get; set; }
        public double unit_price { get; set; }
        public double total_amount { get; set; }
        public double pre_tax_value { get; set; }
        public double discount { get; set; }
        public double other_charge { get; set; }
        public double assessable_value { get; set; }
        public double gst_rate { get; set; }
        public double igst_amount { get; set; }
        public double cgst_amount { get; set; }
        public double sgst_amount { get; set; }
        public double cess_rate { get; set; }
        public double cess_amount { get; set; }
        public double cess_nonadvol_amount { get; set; }
        public double state_cess_rate { get; set; }
        public double state_cess_amount { get; set; }
        public double state_cess_nonadvol_amount { get; set; }
        public double total_item_value { get; set; }
        public string country_origin { get; set; }
        public string order_line_reference { get; set; }
        public string product_serial_number { get; set; }
        public BatchDetails batch_details { get; set; }
        public List<AttributeDetail> attribute_details { get; set; }
    }
    public class RootObject
    {
        public string access_token { get; set; }
        public string user_gstin { get; set; }
        public string data_source { get; set; }
        public TransactionDetails transaction_details { get; set; }
        public DocumentDetails document_details { get; set; }
        public SellerDetails seller_details { get; set; }
        public BuyerDetails buyer_details { get; set; }

        //******************Not Required For Einvoice********************
        public DispatchDetails dispatch_details { get; set; }
        public ShipDetails ship_details { get; set; }
        public ExportDetails export_details { get; set; }
        public PaymentDetails payment_details { get; set; }
        public ReferenceDetails reference_details { get; set; }
        public List<AdditionalDocumentDetail> additional_document_details { get; set; }
        public EwaybillDetails ewaybill_details { get; set; }

        //******************End********************
        public ValueDetails value_details { get; set; }

        public List<ItemList> item_list { get; set; }

    }

    public class Message
    {
        public long AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public string EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
        public string QRCodeUrl { get; set; }
        public string EinvoicePdf { get; set; }
        public string Status { get; set; }
        public string alert { get; set; }
        public bool error { get; set; }
    }

    public class Results
    {
        public Message message { get; set; }
        public string errorMessage { get; set; }
        public string InfoDtls { get; set; }
        public string status { get; set; }
        public int code { get; set; }
        public string requestId { get; set; }
    }

    public class INVRootObject
    {
        public Results results { get; set; }
    }
}
