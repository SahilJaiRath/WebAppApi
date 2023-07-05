using Einv_EwayBill_WebApp.EWBClasses;
using Einv_EwayBill_WebApp.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    public class Taxilla_EinvoiceAPI
    {
        public static async Task<dynamic> AccessToken(DataTable dt)
        {
            dynamic InvResult = "";
            try
            {
                string URL = Taxilla_API_Urls.AuthApi;
                var jsonstring = JsonConvert.SerializeObject("");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("gspappid", dt.Rows[0]["EFUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("gspappsecret", dt.Rows[0]["EFPASSWORD"].ToString());
                    var content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    //HttpResponseMessage response =await client.PostAsync(new Uri(URL), content);
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        dynamic json = JValue.Parse(InvResult);
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(LRData2);
                        //Sekkey = value2.Data.Sek;
                        //Authtoken = value2.Data.AuthToken;
                    }
                    else
                    {
                        InvResult = response.ToString();
                    }
                }
                return InvResult;
            }
            catch (AggregateException err)
            {
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where  and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                return InvResult = err.Message;
            }
        }

       
        public static async Task<dynamic> GenerateIrn(DataTable dt, string jsonfile, string RequestId, string sessionid, string auth,string EINVEWB)
        {
            dynamic InvResult = "";
            string respresult = "";
            try
            {
                string URL = Taxilla_API_Urls.GenIrnApi;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("user_name", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                    //string usrdtl = dt.Rows[0]["EINVUSERNAME"].ToString() + " " + dt.Rows[0]["EINVPASSWORD"].ToString() + " " + dt.Rows[0]["GSTIN"].ToString();
                    //ClsDynamic.WriteLog(usrdtl, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));

                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    //HttpResponseMessage response =await client.PostAsync(new Uri(URL), content);


                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        respresult = InvResult;
                        ClsDynamic.WriteLog(respresult, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        try
                        {
                            var value2 = JsonConvert.DeserializeObject<successRoot>(LRData2);
                            successRoot obj = value2;
                            string infodata = "";
                            if (value2.success == true)
                            {
                                if (obj.info != null)
                                {
                                    infodata = obj.info[0].Desc.ToString();
                                }
                                else
                                {
                                    infodata = "";
                                }

                                string commandText = "UPDATE einvoice_generate_temp SET IRN='" + obj.result.Irn + "',ACKNo='" + obj.result.AckNo + "',ACKDATE='" + obj.result.AckDt + "',SIGNEDQRCODE='" + obj.result.SignedQRCode + "',EWBNO = '" + obj.result.EwbNo + "' , EWBDT = '" + obj.result.EwbDt + "',EWBVALIDTILL = '" + obj.result.EwbValidTill + "' ,QRCODEURL ='' ,EINVOICEPDF ='',ERRORMSG='',ErrorCode='" + infodata + "'  WHERE ID='"+ sessionid + "' and  DOC_NO ='" + dt.Rows[0]["DOC_No"].ToString() + "'";
                                ClsDynamic.WriteLog(commandText, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                                using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                                {
                                    OracleCommand command = new OracleCommand(commandText, connection);
                                    connection.Open();
                                    Int32 rowsAffected = command.ExecuteNonQuery();
                                }

                                respresult = "1";
                                if (value2.result.EwbNo == null && EINVEWB == "Y" && dt.Rows[0]["DOC_TYP"].ToString() == "INV")
                                {
                                    var jfile = Taxilla_EwaybillByIrnClasses.Generate_EwaybillIrn_Json(dt, value2.result.Irn, "");              //Ewaybill by IRN NO  
                                    ClsDynamic.JsonLog(jfile, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                                    var RInvdata = Taxilla_EinvoiceAPI.Generate_EwaybillIrn(dt, jfile, RequestId, sessionid, auth).Result;
                                    if (RInvdata == "1")
                                    {
                                        respresult = "2";
                                    }
                                    else
                                    {
                                        respresult = "3";
                                    }
                                }
                            }
                            else
                            {
                                string DQuote = @"'";
                                respresult = InvResult;
                                ClsDynamic.UpdateErrorLog(respresult, dt.Rows[0]["DOC_NO"].ToString());
                            }

                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                var value2 = JsonConvert.DeserializeObject<successRoot2>(LRData2);
                                successRoot2 obj = value2;
                                string infodata = "";
                                if (value2.success == true)
                                {
                                    if (obj.info != null)
                                    {
                                        infodata = obj.info[0].Desc[0].ErrorMessage.ToString();
                                    }
                                    else
                                    {
                                        infodata = "";
                                    }

                                    string commandText = "UPDATE einvoice_generate_temp SET IRN='" + obj.result.Irn + "',ACKNo='" + obj.result.AckNo + "',ACKDATE='" + obj.result.AckDt + "',SIGNEDQRCODE='" + obj.result.SignedQRCode + "',EWBNO = '" + obj.result.EwbNo + "' , EWBDT = '" + obj.result.EwbDt + "',EWBVALIDTILL = '" + obj.result.EwbValidTill + "' ,QRCODEURL ='' ,EINVOICEPDF ='',ERRORMSG='',ErrorCode='" + infodata + "'  WHERE id='"+sessionid+"' and DOC_NO ='" + dt.Rows[0]["DOC_No"].ToString() + "'";
                                    ClsDynamic.WriteLog(commandText, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                                    {
                                        OracleCommand command = new OracleCommand(commandText, connection);
                                        connection.Open();
                                        Int32 rowsAffected = command.ExecuteNonQuery();
                                    }

                                    respresult = "1";


                                    if (value2.result.EwbNo == null && EINVEWB == "Y" && dt.Rows[0]["DOC_TYP"].ToString() == "INV")
                                    {
                                        var jfile = Taxilla_EwaybillByIrnClasses.Generate_EwaybillIrn_Json(dt, value2.result.Irn, "");              //Ewaybill by IRN NO  
                                        ClsDynamic.JsonLog(jfile, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                                        var RInvdata = Taxilla_EinvoiceAPI.Generate_EwaybillIrn(dt, jfile, RequestId, sessionid, auth).Result;
                                        if (RInvdata == "1")
                                        {
                                            respresult = "2";
                                        }
                                        else
                                        {
                                            respresult = "3";
                                        }
                                    }
                                }
                                else
                                {
                                    string DQuote = @"'";
                                    respresult = InvResult;
                                    ClsDynamic.UpdateErrorLog(respresult, dt.Rows[0]["DOC_NO"].ToString());
                                }
                            }
                            catch (Exception ex1)
                            {
                                try
                                {
                                    var Dupvalue2 = JsonConvert.DeserializeObject<DublicateRoot>(LRData2);
                                    if (Dupvalue2.result[0].Desc.Irn != null)
                                    {
                                        var Irndtl = DetailInvoiceByIrn(dt, Dupvalue2.result[0].Desc.Irn, "", "", auth);
                                    }
                                    else
                                    {
                                        string DQuote = @"'";
                                        respresult = InvResult;
                                        ClsDynamic.UpdateErrorLog(respresult, dt.Rows[0]["DOC_NO"].ToString());
                                    }
                                }
                                catch (Exception ex2)
                                {
                                    string DQuote = @"'";
                                    respresult = InvResult;
                                    ClsDynamic.UpdateErrorLog(respresult, dt.Rows[0]["DOC_NO"].ToString());
                                }
                            }
                        }
                        // string printinimage = value2.result.EwbNo + "" + dt.Rows[0]["GSTIN"].ToString() + "" + value2.result.EwbDt;
                        //ClsDynamic.EwaybillQRcodeImage(value2.result.EwbNo.ToString(), printinimage, "");
                    }
                    else
                    {
                        ClsDynamic.WriteLog(response.ToString(), dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                        string DQuote = @"'";
                        respresult = InvResult;
                        ClsDynamic.UpdateErrorLog(respresult, dt.Rows[0]["DOC_NO"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                string DQuote = @"'";
                respresult = InvResult;
                ClsDynamic.WriteLog(ex.Message, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                ClsDynamic.UpdateErrorLog(respresult, dt.Rows[0]["DOC_NO"].ToString());
            }

            return respresult;
        }

        public static async Task<string> CancelInvoice(DataTable dt, string jsonfile, string RequestId, string type, string auth)
        {
            string InvRData = "";
            dynamic InvResult = "", InvResDecData = "";
            try
            {
                RequestId = ClsDynamic.GenerateUniqueNumber();
                string URL = Taxilla_API_Urls.CanIrnApi;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("user_name", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    //HttpResponseMessage response = client.PostAsJsonAsync(URL, jsonstring).Result;
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        dynamic json = JValue.Parse(InvResult);
                        if(json.success==true)
                        {
                            string LRData2 = await response.Content.ReadAsStringAsync();
                            var value2 = JsonConvert.DeserializeObject<EinvCanSuccessRoot>(LRData2);
                            string sqlstr = "update einvoice_generate set Status='EINVCAN' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                            InvResult = "1";
                        }
                        else
                        {
                            string sqlstr = "update einvoice_generate set ERRORMSG='" + InvResult + "' where  DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                        }                                             
                    }
                }
                return InvRData = InvResult;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate set ERRORMSG='" + InvResult + "' where  DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                return InvRData = ex.Message;
            }

        }

        public static async Task<string> DetailInvoiceByIrn(DataTable dt, string IRN_No, string RequestId, string type, string auth)
        {
            string InvRData = "";
            dynamic InvResult = "";
            try
            {
                string URL = Taxilla_API_Urls.GetInvbyIrn;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("user_name", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                    string param = "irn=" + IRN_No;
                    HttpResponseMessage response = client.GetAsync(URL + param, HttpCompletionOption.ResponseContentRead).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        //var value2 = JsonConvert.DeserializeObject<INVDtlRootObject>(LRData2);
                        //INVDtlRootObject obj = value2;

                        //OracelDataInsert.InsertCancelDataOracle(obj, dt);
                        //SqlConnectionDB.CancelInesrtData(obj, dt);

                    }
                }
                return InvRData = InvResult;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                // ErrorLog.WriteLog(ex);
                return InvRData = ex.Message;
            }

        }

        public static async Task<string> Generate_EwaybillIrn(DataTable dt, string jsonfile, string RequestId, string sessionId, string auth)
        {
            string InvRData = "";
            dynamic InvResult = "", InvResDecData = "";

            try
            {
                RequestId = ClsDynamic.GenerateUniqueNumber();
                string URL = Taxilla_API_Urls.GenEwbbyIrn;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("user_name", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    //HttpResponseMessage response = client.PostAsJsonAsync(URL, jsonstring).Result;
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        ClsDynamic.WriteLog(InvResult, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        dynamic json = JValue.Parse(InvResult);
                        if (json.success == true)
                        {
                            var value2 = JsonConvert.DeserializeObject<IrnEwaybill_successRoot>(LRData2);
                            string sqlstr = "update einvoice_generate_temp set EWBNO='" + value2.result.EwbNo + "',EWBDT='" + value2.result.EwbDt + "',EWBVALIDTILL='" + value2.result.EwbValidTill + "' where id='"+ sessionId + "' and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                            ClsDynamic.WriteLog(sqlstr, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                            InvResult = "1";
                        }
                        else
                        {
                            var rval = GetEwaybillByIrn(dt, dt.Rows[0]["IRN"].ToString(), "", "", auth);
                            if (rval.ToString() != "1")
                            {
                                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + json.message + "' where id='"+sessionId+"' and  DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                            }

                        }
                    }
                }
                return InvRData = InvResult;
            }
            catch (Exception ex)
            {
                ClsDynamic.WriteLog(ex.ToString(), dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));

                string DQuote = @"'";
                string Errmsg = InvResult;
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + Errmsg.Replace(DQuote,"") + "' where id='" + dt.Rows[0]["ID"].ToString() + "' and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                return InvRData = InvResult;
            }

        }

        public static async Task<string> GetEwaybillByIrn(DataTable dt, string IRN_No, string RequestId, string type, string auth)
        {
            string InvRData = "";
            dynamic InvResult = "";
            try
            {
                RequestId = ClsDynamic.GenerateUniqueNumber();
                string URL = Taxilla_API_Urls.GetEwbbyIrn;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("user_name", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                    string param = "irn=" + IRN_No;
                    HttpResponseMessage response = client.GetAsync(URL + param, HttpCompletionOption.ResponseContentRead).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        ClsDynamic.WriteLog(InvResult, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        var ewbresult = JsonConvert.DeserializeObject<GetEwaybillRoot>(LRData2);
                        if (ewbresult.success == true)
                        {
                            string sqlstr = "update einvoice_generate_temp set EWBNO='" + ewbresult.result.EwbNo + "',EWBDT='" + ewbresult.result.EwbDt + "',EWBVALIDTILL='" + ewbresult.result.EwbValidTill + "' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                            ClsDynamic.WriteLog(sqlstr, dt.Rows[0]["DOC_NO"].ToString().Replace("/", "_"));
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);

                            InvResult = "1";
                        }
                       
                    }
                }
                return InvRData = InvResult;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where id='" + dt.Rows[0]["ID"].ToString() + "' and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                // ErrorLog.WriteLog(ex);
                return InvRData = ex.Message;
            }

        }

        public static async Task<string> Cancel_Ewaybill(DataTable dt, string jsonfile, string RequestId, string type, string auth)
        {
            string InvRData = "";
            dynamic InvResult = "", InvResDecData = "";
            try
            {
                string URL = Taxilla_API_Urls.CanEwb;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("user_name", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);


                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    //HttpResponseMessage response = client.PostAsJsonAsync(URL, jsonstring).Result;
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        dynamic json = JValue.Parse(InvResult);
                        if (json.success == true)
                        {
                            var value2 = JsonConvert.DeserializeObject<IrnEwaybill_successRoot>(LRData2);
                            string sqlstr = "update einvoice_generate set status='CAN' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                            InvResult = "1";
                        }
                        else
                        {
                            string sqlstr = "update einvoice_generate set ERRORMSG='" + json.message + "' where  DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                        }
                    }
                }
                return InvRData = InvResult;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate set ERRORMSG='" + InvResult + "' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                return InvRData = ex.Message;
            }
        }

        public static async Task<string> Extract_QRCode(DataTable dt, string jsonfile, string RequestId, string type, string auth)
        {
            string InvRData = "";
            dynamic InvResult = "", InvResDecData = "";
            try
            {
                string URL = Taxilla_API_Urls.ExtQrCode;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("user_name", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    //HttpResponseMessage response = client.PostAsJsonAsync(URL, jsonstring).Result;
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        //var value2 = JsonConvert.DeserializeObject<CancelRoot>(LRData2);
                        // CancelRoot obj = value2;

                        //SqlConnectionDB.CancelInesrtData(obj, dt);
                        // OracelDataInsert.InsertCancelDataOracle(obj, dt);                        
                    }
                }
                return InvRData = InvResult;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                return InvRData = ex.Message;
            }
        }

        public static async Task<string> Extract_SignedInvoice(DataTable dt, string jsonfile, string RequestId, string type, string auth)
        {
            string InvRData = "";
            dynamic InvResult = "", InvResDecData = "";
            try
            {
                string URL = Taxilla_API_Urls.ExtSignInv;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("user_name", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);

                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    //HttpResponseMessage response = client.PostAsJsonAsync(URL, jsonstring).Result;
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        //var value2 = JsonConvert.DeserializeObject<CancelRoot>(LRData2);
                        // CancelRoot obj = value2;

                        //SqlConnectionDB.CancelInesrtData(obj, dt);
                        // OracelDataInsert.InsertCancelDataOracle(obj, dt);                        
                    }
                }
                return InvRData = InvResult;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                return InvRData = ex.Message;
            }

        }

        public static async Task<string> Generate_QRImage(DataTable dt, string jsonfile, string RequestId, string type, string auth)
        {
            string InvRData = "";
            dynamic InvResult = "", InvResDecData = "";
            try
            {
                string URL = Taxilla_API_Urls.GenQRImg;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("user_name", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);


                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    //HttpResponseMessage response = client.PostAsJsonAsync(URL, jsonstring).Result;
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        //var value2 = JsonConvert.DeserializeObject<CancelRoot>(LRData2);
                        // CancelRoot obj = value2;

                        //SqlConnectionDB.CancelInesrtData(obj, dt);
                        // OracelDataInsert.InsertCancelDataOracle(obj, dt);                        
                    }
                }
                return InvRData = InvResult;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                return InvRData = ex.Message;
            }

        }


        public static async Task<string> Get_QRImage(DataTable dt, string jsonfile, string RequestId, string type, string auth)
        {
            string InvRData = "";
            dynamic InvResult = "", InvResDecData = "";
            try
            {
                string URL = Taxilla_API_Urls.GetQRImg;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("user_name", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("width", "250");
                    client.DefaultRequestHeaders.Add("height", "250");
                    client.DefaultRequestHeaders.Add("imgtype", "jpg");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                    client.DefaultRequestHeaders.Add("irn", dt.Rows[0]["IRN"].ToString());

                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    //HttpResponseMessage response = client.PostAsJsonAsync(URL, jsonstring).Result;
                    //string param = "irn=" + IRN_No;
                    HttpResponseMessage response = client.GetAsync(URL, HttpCompletionOption.ResponseContentRead).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        //var value2 = JsonConvert.DeserializeObject<CancelRoot>(LRData2);
                        // CancelRoot obj = value2;

                        //SqlConnectionDB.CancelInesrtData(obj, dt);
                        // OracelDataInsert.InsertCancelDataOracle(obj, dt);                        
                    }
                }
                return InvRData = InvResult;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                return InvRData = ex.Message;
            }

        }
    }
}
