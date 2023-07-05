using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Einv_EwayBill_WebApp.EWBClasses;
using Einv_EwayBill_WebApp.Models;
using Einv_EwayBill_WebApp.ViewModels;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace Einv_EwayBill_WebApp.Controllers
{
    public class EinvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]       
        public IActionResult GenEinvoice (string ptype,string DocumentId,string dbunit,string CanCode,string Eitype)
        {
            string Provider = "TAXILLA";
            string dbname="",Pwd="",DbUser="",dbServer="";
            //ptype = "GINV";
            //Type = "NUSER";
            List<dbdetailsmodel> dbdtl = new List<dbdetailsmodel>();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "ExcelFile", "dbserverdtl.xlsx");
            if (System.IO.File.Exists(path))
            {               
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        while (reader.Read()) //Each ROW
                        {
                            dbdtl.Add(new dbdetailsmodel
                            {
                                unitcode = reader.GetValue(0).ToString(),
                                dbname = reader.GetValue(1).ToString(),
                                dbuser = reader.GetValue(2).ToString(),
                                dbpassword= reader.GetValue(3).ToString(),
                                serverip= reader.GetValue(4).ToString()
                            });
                        }
                    }
                }
            }

            var filtereddbdtl = dbdtl.Where(user => user.unitcode== dbunit);
            foreach (dbdetailsmodel user in filtereddbdtl)
            {
                dbname = user.dbname;
                DbUser = user.dbuser;
                Pwd = user.dbpassword;
                dbServer = user.serverip;

            }
                //Console.WriteLine(user + ": " + user.Age);

            string AppType = "";
            if (string.IsNullOrEmpty(ptype) || string.IsNullOrEmpty(DocumentId) || string.IsNullOrEmpty(dbname) || string.IsNullOrEmpty(Pwd) || string.IsNullOrEmpty(DbUser)|| string.IsNullOrEmpty(dbServer)||string.IsNullOrEmpty(Eitype))
            {
                ViewBag.Processed = "Parameter provided should not be null or Empty";
                return View();
            }

            if(ptype=="CINV")
            {
                if(Convert.ToInt32(CanCode)<=0 || Convert.ToInt32(CanCode) > 4)
                {
                    ViewBag.Processed = "While Cancel Einvoive Cancel code should be >0 and <=4 ";
                    return View();
                }
            }

            OraDBConnection.SetConnectiondata(DbUser, Pwd,  ""+dbServer+":1521/"+ dbname + "");
            //OracleConnection con = new OracleConnection(OraDBConnection.OrclConnection);
            //con.Open();
            //ViewBag.Processed="Connected with Oracle";
            //con.Close();
            try
            {

                DataTable dtapptype = GetDataFromDB.GetDistinctData("APPTYPE", DocumentId, Eitype);
                if(dtapptype.Rows.Count>0)
                {
                    AppType = dtapptype.Rows[0]["value_1"].ToString();

                    DataTable dtinvewb = new DataTable();                   

                    DataTable dt1 = GetDataFromDB.GetDistinctData("GETID", DocumentId, Eitype);
                    if(dt1.Rows.Count>0)
                    {
                        dtinvewb = dt1;
                    }
                    else
                    {
                        dtinvewb = GetDataFromDB.GetDistinctData("CHECKINVEWB", DocumentId, Eitype);
                    }

                    
                    if (dtinvewb.Rows.Count > 0)
                    {
                        if (AppType == "B" && ptype.ToUpper() == "GEWB")
                        {
                            string Rdata = dtinvewb.Rows[0]["ITEM_BARCDE"].ToString();
                            if (Rdata == "CH")
                            {
                                AppType = "EW";
                            }
                        }
                        else if (AppType == "B" && ptype.ToUpper() == "CEWB")
                        {
                            AppType = "EW";
                        }
                        else if (AppType == "B" && (dtinvewb.Rows[0]["BILLFROM_GSTIN"].ToString() == dtinvewb.Rows[0]["BILLTO_GSTIN"].ToString()))
                        {
                            AppType = "EW";
                            ptype = "GEWB";
                        }
                    }
                    else
                    {
                        ViewBag.Processed = "No Data found for Ewaybill ";
                    }
                }
                else
                {
                    ViewBag.Processed = "Parameter type is not define";
                    return View();
                }

                DataTable dtInvL = new DataTable();
                if (Provider.ToUpper()== "TAXILLA")
                {               
                    if (AppType.ToUpper() == "EI")
                    {
                        if (ptype.ToUpper().Trim() == "GINV")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("GINV", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (auth != "")
                                {
                                    for (int i = 0; i < dtInvL.Rows.Count; i++)
                                    {
                                        DataTable dtinv = new DataTable();
                                        dtinv = GetDataFromDB.GetEinvoiceData("GINV", DocumentId, dtInvL.Rows[i]["DOC_No"].ToString());
                                        if (dtinv.Rows.Count > 0)
                                        {                                            
                                            var jfile = Taxilla_GenerateEinvoice.GenerateIRNJsonFile(dtinv,"N");  
                                            if(jfile!="")
                                            {
                                                ClsDynamic.JsonLog(jfile, dtInvL.Rows[0]["DOC_No"].ToString().Replace("/","_"));
                                                string finalString = ClsDynamic.GenerateUniqueNumber();
                                                var RInvdata = Taxilla_EinvoiceAPI.GenerateIrn(dtinv, jfile, finalString, DocumentId, auth,"N").Result;
                                                if (RInvdata == "1")
                                                {
                                                    ViewBag.Processed = "Einvoice Generated successfully";
                                                }
                                                else
                                                {
                                                    ViewBag.Processed = RInvdata;
                                                }
                                            }
                                            else
                                            {
                                                ViewBag.Processed = jfile;
                                            }
                                           
                                        }
                                        else
                                        {
                                            ViewBag.Processed = "No Data Found";
                                        }
                                    }
                                }
                                else
                                {
                                    ViewBag.Processed = "Accesstoken not generated please check error message in your table "+ access_token;
                                }
                            }
                            else
                            {
                                ViewBag.Processed = "No Data Found";
                            }

                        }
                        else if (ptype.ToUpper().Trim() == "CINV")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("CINV", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    for (int i = 0; i < dtInvL.Rows.Count; i++)
                                    {
                                        string finalString = ClsDynamic.GenerateUniqueNumber();
                                        var jfile = Taxilla_CancelIrnClasses.CancelJsonFile(dtInvL, DocumentId, access_token,CanCode);
                                        var RInvdata = Taxilla_EinvoiceAPI.CancelInvoice(dtInvL, jfile, finalString, tokentype, auth).Result;
                                        if (RInvdata == "1")
                                        {
                                            ViewBag.Processed = "Einvoice Canceled";
                                        }
                                        else
                                        {
                                            ViewBag.Processed = RInvdata;
                                        }                                       
                                    } 
                                }
                                else
                                {
                                    ViewBag.Processed = "Accesstoken not generated please check error message in your table";
                                }
                            }
                            else
                            {
                                ViewBag.Processed = "No Data Found";
                            }
                        }

                        else if (ptype.ToUpper().Trim() == "GETINVBYIRN")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("GETINVBYIRN", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                string finalString = ClsDynamic.GenerateUniqueNumber();
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {                                 
                                    var RInvdata = Taxilla_EinvoiceAPI.DetailInvoiceByIrn(dtInvL, DocumentId, finalString, tokentype, auth).Result;
                                }

                            }
                        }

                        else if (ptype.ToUpper().Trim() == "EWBBYIRN")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("EWBBYIRN", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    string finalString = ClsDynamic.GenerateUniqueNumber();
                                    var jfile = Taxilla_EwaybillByIrnClasses.Generate_EwaybillIrn_Json(dtInvL, DocumentId, access_token);                                    
                                    var RInvdata = Taxilla_EinvoiceAPI.Generate_EwaybillIrn(dtInvL, jfile, finalString, tokentype, auth).Result;
                                }

                            }
                        }

                        else if (ptype.ToUpper().Trim() == "CEWBBYIRN")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("CEWBBYIRN", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    string finalString = ClsDynamic.GenerateUniqueNumber();
                                    var jfile = Taxilla_EwaybillByIrnClasses.Cancel_Ewaybill_Json(dtInvL, DocumentId, access_token);                                    
                                    var RInvdata = Taxilla_EinvoiceAPI.Cancel_Ewaybill(dtInvL, jfile, finalString, tokentype, auth).Result;
                                }

                            }
                        }

                        else if (ptype.ToUpper().Trim() == "EXTQR")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("EXTQR", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    string finalString = ClsDynamic.GenerateUniqueNumber();
                                    var jfile = Taxilla_EwaybillByIrnClasses.ExtractQR_Code_Json(dtInvL, DocumentId, access_token);                                    
                                    var RInvdata = Taxilla_EinvoiceAPI.Extract_QRCode(dtInvL, jfile, finalString, tokentype, auth).Result;
                                }

                            }
                        }

                        else if (ptype.ToUpper().Trim() == "EXTINV")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("EXTINV", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    string finalString = ClsDynamic.GenerateUniqueNumber();
                                    var jfile = Taxilla_EwaybillByIrnClasses.Extract_SignedInvoice_Json(dtInvL, DocumentId, access_token);                                    
                                    var RInvdata = Taxilla_EinvoiceAPI.Extract_SignedInvoice(dtInvL, jfile, finalString, tokentype, auth).Result;
                                }

                            }
                        }

                        else if (ptype.ToUpper().Trim() == "EXTINV")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("EXTINV", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    string finalString = ClsDynamic.GenerateUniqueNumber();
                                    var jfile = Taxilla_EwaybillByIrnClasses.Extract_SignedInvoice_Json(dtInvL, DocumentId, access_token);                                    
                                    var RInvdata = Taxilla_EinvoiceAPI.Extract_SignedInvoice(dtInvL, jfile, finalString, tokentype, auth).Result;
                                }

                            }
                        }

                        else if (ptype.ToUpper().Trim() == "GQRIMG")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("GQRIMG", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    string finalString = ClsDynamic.GenerateUniqueNumber();
                                    var jfile = Taxilla_EwaybillByIrnClasses.ExtractQR_Code_Json(dtInvL, DocumentId, access_token);                                    
                                    var RInvdata = Taxilla_EinvoiceAPI.Generate_QRImage(dtInvL, jfile, finalString, tokentype, auth).Result;
                                }

                            }
                        }

                        else if (ptype.ToUpper().Trim() == "GetQRIMG")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("GetQRIMG", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    string finalString = ClsDynamic.GenerateUniqueNumber();
                                    var RInvdata = Taxilla_EinvoiceAPI.Get_QRImage(dtInvL, "", finalString, tokentype, auth).Result;
                                }
                            }
                        }
                    }
                    else if(AppType.ToUpper()=="EW")
                    {
                        if (ptype.ToUpper() == "GEWB")
                        {
                            DataTable dtEWBL = new DataTable();
                            dtEWBL = Taxilla_EWBGetData.GetDistinctData("GEWB", DocumentId);
                            //dynamic access_token = GetAccessToken();
                            if (dtEWBL.Rows.Count > 0)
                            {
                                //var access_token = Taxilla_EwaybillAPI.EwayAccessToken(dtEWBL).Result;
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtEWBL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (auth != "")
                                {
                                    for (int i = 0; i < dtEWBL.Rows.Count; i++)
                                    {                                        
                                        DataTable dtEWB = new DataTable();
                                        dtEWB = Taxilla_EWBGetData.GetEWBData("GEWB", DocumentId, dtEWBL.Rows[i]["DOC_NO"].ToString());
                                        if (dtEWB.Rows.Count > 0)
                                        {
                                            var jsondata = Taxilla_GenEwaybillClasses.EWBJsonFile(dtEWB, access_token);
                                            if(jsondata!="")
                                            {
                                                ClsDynamic.JsonLog(jsondata, "EWB_" + dtEWBL.Rows[i]["DOC_NO"].ToString().Replace("/", "_"));
                                                //ErrorLog.WriteJsonFile(jsondata);
                                                string finalString = ClsDynamic.GenerateUniqueNumber();
                                                var RInvdata = Taxilla_EwaybillAPI.GenEwayBill(dtEWB, jsondata, finalString, tokentype, auth).Result;
                                                if(RInvdata=="1")
                                                {
                                                    ViewBag.Processed = "Ewaybill Generated Successfully";
                                                }
                                                else
                                                {
                                                    ViewBag.Processed = "Some Error While generating ewaybill "+ RInvdata;
                                                }                                                
                                            }
                                            else
                                            {
                                                ViewBag.Processed = "Some Error While generating ewaybill json file ";
                                            }                                           
                                        }
                                    }
                                }
                                else
                                {
                                    ViewBag.Processed = "Accesstoken not generated please check error message in your table";
                                }

                            }
                            else
                            {
                                ViewBag.Processed = "No Data Found";
                            }
                        }

                        else if (ptype.ToUpper() == "CEWB")
                        {
                            DataTable dtEWBL = new DataTable();
                            dtEWBL = Taxilla_EWBGetData.GetDistinctData("CEWB", DocumentId);
                            //dynamic access_token = GetAccessToken();
                            if (dtEWBL.Rows.Count > 0)
                            {
                                //var access_token = Taxilla_EwaybillAPI.EwayAccessToken(dtEWBL).Result;
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtEWBL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (auth != "")
                                {
                                    for (int i = 0; i < dtEWBL.Rows.Count; i++)
                                    {
                                        string finalString = ClsDynamic.GenerateUniqueNumber();
                                        DataTable dtEWB = new DataTable();
                                        dtEWB = Taxilla_EWBGetData.GetEWBData("CEWB", dtEWBL.Rows[0]["EWBNO"].ToString(), dtEWBL.Rows[0]["DOC_NO"].ToString());
                                        if (dtEWB.Rows.Count > 0)
                                        {
                                            var jsondata = Taxilla_CanEwaybillClasses.GenerateCancelJson(dtEWB, access_token);
                                            //ErrorLog.WriteJsonFile(jsondata);
                                            var RInvdata = Taxilla_EwaybillAPI.CancelEwayBill(dtEWB, jsondata, finalString, tokentype, auth).Result;
                                            if(RInvdata=="1")
                                            {
                                                ViewBag.Processed = "Ewaybill canceled";
                                            }
                                            else
                                            {
                                                ViewBag.Processed = RInvdata;
                                            }
                                            
                                        }
                                    }
                                }
                                else
                                {
                                    ViewBag.Processed = "Accesstoken not generated please check error message in your table";
                                }
                            }
                            else
                            {
                                ViewBag.Processed = "No Data Found";
                            }
                        }
                    }
                    else if(AppType.ToUpper()=="B")
                    {
                        if (ptype.ToUpper().Trim() == "GINV")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("GINV", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (auth != "")
                                {
                                    for (int i = 0; i < dtInvL.Rows.Count; i++)
                                    {
                                        
                                        DataTable dtinv = new DataTable();
                                        dtinv = GetDataFromDB.GetEinvoiceData("GINV", DocumentId, dtInvL.Rows[i]["DOC_No"].ToString());
                                        if (dtinv.Rows.Count > 0)
                                        {
                                            if(dtInvL.Rows[i]["IRN"].ToString()=="")
                                            {
                                                var jfile = Taxilla_GenerateEinvoice.GenerateIRNJsonFile(dtinv, "Y");
                                                if(jfile!="")
                                                {
                                                    string finalString = ClsDynamic.GenerateUniqueNumber();
                                                    var RInvdata = Taxilla_EinvoiceAPI.GenerateIrn(dtinv, jfile, finalString, DocumentId, auth, "Y").Result;
                                                    if (RInvdata == "1" || RInvdata == "2")
                                                    {
                                                        ViewBag.Processed = "Einvoice and Eway bill Generated successfully";
                                                    }
                                                    else if (RInvdata == "3")
                                                    {
                                                        ViewBag.Processed = "Einvoice Generated successfully and Some Error while Generating Ewaybill so Please check Error message and Try Again";
                                                    }
                                                    else
                                                    {
                                                        ViewBag.Processed = RInvdata;
                                                    }
                                                }
                                                else
                                                {
                                                    ViewBag.Processed ="Error while generating in json file "+ jfile.ToString();
                                                }
                                               
                                            }
                                            else
                                            {
                                                if(dtinv.Rows[i]["IRN"].ToString()!="")
                                                {
                                                    var jfile = Taxilla_EwaybillByIrnClasses.Generate_EwaybillIrn_Json(dtinv, dtinv.Rows[i]["IRN"].ToString(), "");
                                                    if (jfile != "")
                                                    {
                                                        ClsDynamic.JsonLog(jfile, "EWBBYIRN_" + dtInvL.Rows[i]["DOC_No"].ToString());
                                                        string finalString = ClsDynamic.GenerateUniqueNumber();
                                                        var RInvdata = Taxilla_EinvoiceAPI.Generate_EwaybillIrn(dtinv, jfile, finalString, DocumentId, auth).Result;
                                                        if (RInvdata == "1")
                                                        {
                                                            ViewBag.Processed = "Eway bill Generated successfully";
                                                        }
                                                        else
                                                        {
                                                            ViewBag.Processed = RInvdata;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ViewBag.Processed = "Error while generating in json file ";
                                                    }
                                                }
                                                //else
                                                //{
                                                //    DataTable dtEWBL = new DataTable();
                                                //    dtEWBL = Taxilla_EWBGetData.GetDistinctData("GEWB", DocumentId);
                                                //    if (dtEWBL.Rows.Count > 0)
                                                //    {
                                                //        var access_token1 = Taxilla_EinvoiceAPI.AccessToken(dtEWBL).Result;
                                                //        var value21 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                                //        string requestid1 = value21.jti;
                                                //        string tokentype1 = value21.token_type;
                                                //        string auth1 = value21.access_token;
                                                //        if (auth != "")
                                                //        {
                                                //            for (int k = 0; k < dtEWBL.Rows.Count; i++)
                                                //            {
                                                //                DataTable dtEWB = new DataTable();
                                                //                dtEWB = Taxilla_EWBGetData.GetEWBData("GEWB", DocumentId, dtEWBL.Rows[k]["DOC_NO"].ToString());
                                                //                if (dtEWB.Rows.Count > 0)
                                                //                {
                                                //                    var jsondata1 = Taxilla_GenEwaybillClasses.EWBJsonFile(dtEWB, access_token);
                                                //                    if (jsondata1 != "")
                                                //                    {
                                                //                        ClsDynamic.JsonLog(jsondata1, "EWB_" + dtEWBL.Rows[k]["DOC_NO"].ToString().Replace("/", "_"));
                                                //                        //ErrorLog.WriteJsonFile(jsondata);
                                                //                        string finalString = ClsDynamic.GenerateUniqueNumber();
                                                //                        var RInvdata = Taxilla_EwaybillAPI.GenEwayBill(dtEWB, jsondata1, finalString, tokentype1, auth1).Result;
                                                //                        if (RInvdata == "1")
                                                //                        {
                                                //                            ViewBag.Processed = "Ewaybill Generated Successfully";
                                                //                        }
                                                //                        else
                                                //                        {
                                                //                            ViewBag.Processed = "Some Error While generating ewaybill " + RInvdata;
                                                //                        }
                                                //                    }
                                                //                    else
                                                //                    {
                                                //                        ViewBag.Processed = "Some Error While generating ewaybill json file ";
                                                //                    }
                                                //                }
                                                //            }
                                                //        }
                                                //        else
                                                //        {
                                                //            ViewBag.Processed = "Accesstoken not generated please check error message in your table";
                                                //        }

                                                //    }
                                                //    else
                                                //    {
                                                //        ViewBag.Processed = "No Data Found";
                                                //    }
                                                //}
                                                                                          
                                            }                                                                               
                                        }                                        
                                    }
                                }
                                else
                                {
                                    ViewBag.Processed = "Accesstoken not generated please check error message in your table "+access_token;
                                }
                            }
                            else
                            {
                                ViewBag.Processed = "No Data Found";
                            }

                        }
                        else if (ptype.ToUpper().Trim() == "CINV")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("CINV", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    for (int i = 0; i < dtInvL.Rows.Count; i++)
                                    {
                                        if(dtInvL.Rows[i]["EWBNO"].ToString()!="")
                                        {
                                            string finalString = ClsDynamic.GenerateUniqueNumber();
                                            var CEWbJfile = Taxilla_EwaybillByIrnClasses.Cancel_Ewaybill_Json(dtInvL, DocumentId, access_token);
                                            //var CEWBRInvdata = Taxilla_EinvoiceAPI.Cancel_Ewaybill(dtInvL, CEWbJfile, finalString, tokentype, auth).Result;
                                            var CEWBRInvdata = Taxilla_EwaybillAPI.CancelEwayBill(dtInvL, CEWbJfile, finalString, tokentype, auth).Result;
                                            if (CEWBRInvdata == "1")
                                            {
                                                var jfile = Taxilla_CancelIrnClasses.CancelJsonFile(dtInvL, DocumentId, access_token, CanCode);
                                                var RInvdata = Taxilla_EinvoiceAPI.CancelInvoice(dtInvL, jfile, finalString, tokentype, auth).Result;
                                                if (RInvdata == "1")
                                                {
                                                    ViewBag.Processed = "Eway bill and Einvoice Canceled";
                                                }
                                                else
                                                {
                                                    ViewBag.Processed = CEWBRInvdata;
                                                }
                                            }
                                            else
                                            {
                                                ViewBag.Processed = CEWBRInvdata;
                                            }
                                        }
                                        else
                                        {
                                            string finalString = ClsDynamic.GenerateUniqueNumber();
                                            var jfile = Taxilla_CancelIrnClasses.CancelJsonFile(dtInvL, DocumentId, access_token, CanCode);
                                            var RInvdata = Taxilla_EinvoiceAPI.CancelInvoice(dtInvL, jfile, finalString, tokentype, auth).Result;
                                            if (RInvdata == "1")
                                            {
                                                ViewBag.Processed = "Einvoice Canceled";
                                            }
                                            else
                                            {
                                                ViewBag.Processed = RInvdata;
                                            }
                                        }
                                        
                                    }
                                }
                                else
                                {
                                    ViewBag.Processed = "Accesstoken not generated please check error message in your table";
                                }
                            }
                            else
                            {
                                ViewBag.Processed = "No data Found";
                            }
                        }

                        else if (ptype.ToUpper().Trim() == "GETINVBYIRN")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("GETINVBYIRN", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    string finalString = ClsDynamic.GenerateUniqueNumber();
                                    var RInvdata = Taxilla_EinvoiceAPI.DetailInvoiceByIrn(dtInvL, DocumentId, finalString, tokentype, auth).Result;
                                }

                            }
                        }

                        else if (ptype.ToUpper().Trim() == "EWBBYIRN")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("EWBBYIRN", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    for (int i = 0; i < dtInvL.Rows.Count; i++)
                                    {
                                        string finalString = ClsDynamic.GenerateUniqueNumber();
                                        DataTable dtinv = new DataTable();
                                        dtinv = GetDataFromDB.GetEinvoiceData("EWBBYIRN", DocumentId, dtInvL.Rows[i]["DOC_No"].ToString());
                                        if (dtinv.Rows.Count > 0)
                                        {
                                            var jfile = Taxilla_EwaybillByIrnClasses.Generate_EwaybillIrn_Json(dtinv, DocumentId, access_token);
                                            var RInvdata = Taxilla_EinvoiceAPI.Generate_EwaybillIrn(dtinv, jfile, finalString, tokentype, auth).Result;

                                            if(RInvdata=="1")
                                            {
                                                ViewBag.Processed = "Eway bill Generated successfully by IRN";
                                            }
                                            else
                                            {
                                                ViewBag.Processed = RInvdata;
                                            }
                                        }
                                    }                                            
                                }
                            }
                            else
                            {
                                ViewBag.Processed = "No Data Found";
                            }
                        }

                        else if (ptype.ToUpper().Trim() == "CEWBBYIRN")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("CEWBBYIRN", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = Taxilla_EinvoiceAPI.AccessToken(dtInvL).Result;
                                var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(access_token);
                                string requestid = value2.jti;
                                string tokentype = value2.token_type;
                                string auth = value2.access_token;
                                if (access_token != "")
                                {
                                    for (int i = 0; i < dtInvL.Rows.Count; i++)
                                    {
                                        string finalString = ClsDynamic.GenerateUniqueNumber();
                                        var jfile = Taxilla_EwaybillByIrnClasses.Cancel_Ewaybill_Json(dtInvL, DocumentId, access_token);
                                        var RInvdata = Taxilla_EinvoiceAPI.Cancel_Ewaybill(dtInvL, jfile, finalString, tokentype, auth).Result;
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                else
                {
                    if(AppType=="EI")
                    {
                        if (ptype.ToUpper() == "GINV")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("GINV", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                var access_token = GetAccessToken.Einvoice_API_Login(dtInvL).Result;
                                if (access_token != "")
                                {
                                    for (int i = 0; i < dtInvL.Rows.Count; i++)
                                    {

                                        DataTable dtinv = new DataTable();
                                        dtinv = GetDataFromDB.GetEinvoiceData("GINV", DocumentId, dtInvL.Rows[i]["DOC_No"].ToString());
                                        if (dtinv.Rows.Count > 0)
                                        {
                                            var jfile = GenerateEinvoice.GenerateJsonFile(dtinv, access_token);
                                            var RInvdata = GenerateEinvoice.generateInvoice(jfile, access_token, dtinv).Result;
                                            if (RInvdata == "1")
                                            {
                                                Console.WriteLine("einvoice generated");
                                            }
                                            else
                                            {
                                                Console.WriteLine(RInvdata);
                                            }

                                        }
                                        else
                                        {
                                            Console.WriteLine("No Data Found");
                                        }
                                    }
                                }
                            }
                        }
                        else if (ptype == "CINV")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("CINV", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                dynamic access_token = GetAccessToken.Einvoice_API_Login(dtInvL).Result;
                                if (access_token != "")
                                {
                                    var jfile = CanEinvoice.CancelJsonFile(dtInvL, DocumentId, access_token);
                                    var RInvdata = CanEinvoice.CancelInvoice(jfile, access_token, dtInvL).Result;
                                }
                            }
                        }
                        else if (ptype == "DTLINV")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("DTLINV", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                dynamic access_token = GetAccessToken.Einvoice_API_Login(dtInvL).Result;
                                if (access_token != "")
                                {
                                    DataTable dtinv = new DataTable();
                                    dtinv = GetDataFromDB.GetEinvoiceData("DTLINV", DocumentId, dtInvL.Rows[0]["DOC_No"].ToString());
                                    if (dtinv.Rows.Count > 0)
                                    {
                                        var RInvdata = DtlInvoiceByIrn.DetailInvoiceByIrn(DocumentId, access_token, dtinv).Result;
                                    }
                                }

                            }
                        }
                        else if (ptype == "EWBBYIRN")
                        {
                            dtInvL = GetDataFromDB.GetDistinctData("EWBBYIRN", DocumentId, Eitype);
                            if (dtInvL.Rows.Count > 0)
                            {
                                dynamic access_token = GetAccessToken.Einvoice_API_Login(dtInvL).Result;
                                if (access_token != "")
                                {
                                    var jfile = EwaybillByIrn.Generate_EwaybillIrn_Json(dtInvL, DocumentId, access_token);
                                    var RInvdata = EwaybillByIrn.Generate_EwaybillIrn(jfile, access_token, dtInvL, ptype).Result;
                                }
                            }
                        }
                    } 
                }                    
            }
            catch(Exception ex)
            {
                ViewBag.Processed = ex.Message;
            }          
            return View();
        }
       
    }
}
