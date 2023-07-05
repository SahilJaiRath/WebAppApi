using Einv_EwayBill_WebApp.EWBClasses;
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
    public class Taxilla_EwaybillAPI
    {
        public static async Task<dynamic> EwayAccessToken(DataTable dt)
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
                    client.DefaultRequestHeaders.Add("gspappid", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("gspappsecret", dt.Rows[0]["EINVPASSWORD"].ToString());
                    var content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    //HttpResponseMessage response =await client.PostAsync(new Uri(URL), content);
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;

                        dynamic json = JValue.Parse(InvResult);
                        if (json.Status == 0)
                        {
                            string ErrorMsg = "Error";
                        }
                        else
                        {
                            string LRData2 = await response.Content.ReadAsStringAsync();
                            var value2 = JsonConvert.DeserializeObject<AccessTokenResult>(LRData2);
                            //Sekkey = value2.Data.Sek;
                            //Authtoken = value2.Data.AuthToken;
                        }
                    }

                }
                return InvResult;
            }
            catch (AggregateException err)
            {
                foreach (var errInner in err.InnerExceptions)
                {
                    Console.WriteLine(errInner.ToString());
                    // Debug.WriteLine(errInner); //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                }
                return InvResult = err.Message;
            }
        }

        public static async Task<dynamic> GenerateEwayBill(DataTable dt, string jsonfile, string RequestId, string type, string auth)
        {
            dynamic InvResult = "";
            try
            {
                string URL = Taxilla_API_Urls.GenEwaybillApi;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("username", dt.Rows[0]["EFUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EFPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", auth);

                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    //HttpResponseMessage response =await client.PostAsync(new Uri(URL), content);
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        var value2 = JsonConvert.DeserializeObject<EwaySuccessRoot>(LRData2);
                        if (value2.success == true)
                        {
                            string commandText = "UPDATE EWB_GEN_STD SET EWAY_BILL_NO=:EWAY_BILL_NO,EWAY_BILL_DATE=:EWAY_BILL_DATE,PDF_URL=:PDF_URL,ERRORMSG =:ERRORMSG " + " WHERE DOCNO = :DOCNO";
                            using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                            {
                                OracleCommand command = new OracleCommand(commandText, connection);

                                command.Parameters.Add(":EWAY_BILL_NO", OracleDbType.Varchar2);
                                command.Parameters[":EWAY_BILL_NO"].Value = value2.result.ewayBillNo;
                                command.Parameters.Add(":EWAY_BILL_DATE", OracleDbType.Varchar2);
                                command.Parameters[":EWAY_BILL_DATE"].Value = value2.result.ewayBillDate;
                                command.Parameters.Add(":PDF_URL", OracleDbType.Varchar2);
                                command.Parameters[":PDF_URL"].Value = value2.message;

                                // Use AddWithValue to assign Demographics.
                                command.Parameters.Add(":ERRORMSG", OracleDbType.Varchar2);
                                command.Parameters[":ERRORMSG"].Value = value2.result.alert;


                                command.Parameters.Add(":DOCNO", OracleDbType.Varchar2);
                                command.Parameters[":DOCNO"].Value = dt.Rows[0]["DOCNO"].ToString();

                                connection.Open();
                                Int32 rowsAffected = command.ExecuteNonQuery();
                                //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                                InvResult = "1";
                            }
                        }
                        else
                        {
                            string sqlstr = "update EWB_GEN_STD set ERRORMSG='" + InvResult + "' where id='" + dt.Rows[0]["ID"].ToString() + "' and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                            //ErrorLog.WriteLog(ex);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string sqlstr = "update EWB_GEN_STD set ERRORMSG='" + InvResult + "' where id='" + dt.Rows[0]["ID"].ToString() + "' and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                InvResult = ex.Message;
            }

            return InvResult;
        }

        public static async Task<dynamic> GenEwayBill(DataTable dt, string jsonfile, string RequestId, string type, string auth)
        {
            dynamic InvResult = "";
            try
            {
                string URL = Taxilla_API_Urls.GenEwaybillApi;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("username", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", auth);

                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    //HttpResponseMessage response =await client.PostAsync(new Uri(URL), content);
                    if (response.IsSuccessStatusCode)
                    {
                        InvResult = response.Content.ReadAsStringAsync().Result;
                        ClsDynamic.WriteLog(InvResult,"EWB_"+dt.Rows[0]["DOC_NO"].ToString());
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        var value2 = JsonConvert.DeserializeObject<EwaySuccessRoot>(LRData2);
                        if (value2.success == true)
                        {
                            string commandText = "UPDATE einvoice_generate_temp SET EWBNO=:EWBNO,EWBDT=:EWBDT,EWBVALIDTILL=:EWBVALIDTILL,ERRORMSG =:ERRORMSG " + " WHERE DOC_NO = :DOC_NO";
                            using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                            {
                                OracleCommand command = new OracleCommand(commandText, connection);

                                command.Parameters.Add(":EWBNO", OracleDbType.Varchar2);
                                command.Parameters[":EWBNO"].Value = value2.result.ewayBillNo;
                                command.Parameters.Add(":EWBDT", OracleDbType.Varchar2);
                                command.Parameters[":EWBDT"].Value = value2.result.ewayBillDate;
                                command.Parameters.Add(":EWBVALIDTILL", OracleDbType.Varchar2);
                                command.Parameters[":EWBVALIDTILL"].Value = value2.result.validUpto;

                                // Use AddWithValue to assign Demographics.
                                command.Parameters.Add(":ERRORMSG", OracleDbType.Varchar2);
                                command.Parameters[":ERRORMSG"].Value = value2.result.alert;

                                command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                                command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_NO"].ToString();

                                connection.Open();
                                Int32 rowsAffected = command.ExecuteNonQuery();
                                //Console.WriteLine("RowsAffected: {0}", rowsAffected);

                                InvResult = "1";
                            }
                        }
                        else
                        {
                            string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where id='" + dt.Rows[0]["ID"].ToString() + "' and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                            //ErrorLog.WriteLog(ex);
                        }
                    }
                    else
                    {
                        InvResult = response.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + InvResult + "' where id='" + dt.Rows[0]["ID"].ToString() + "' and DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                InvResult = ex.Message;
            }

            return InvResult;
        }

        public static async Task<dynamic> CancelEwayBill(DataTable dt, string jsonfile, string RequestId, string type, string auth)
        {
            dynamic InvResult = "", InvResDecData = "";

            try
            {
                string URL = Taxilla_API_Urls.CANEWBApi;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("username", dt.Rows[0]["EINVUSERNAME"].ToString());
                    client.DefaultRequestHeaders.Add("password", dt.Rows[0]["EINVPASSWORD"].ToString());
                    client.DefaultRequestHeaders.Add("gstin", dt.Rows[0]["GSTIN"].ToString());
                    client.DefaultRequestHeaders.Add("requestid", RequestId);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", auth);

                    var content = new StringContent(jsonfile, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(new Uri(URL), content).Result;
                    //HttpResponseMessage response =await client.PostAsync(new Uri(URL), content);
                    if (response.IsSuccessStatusCode)
                    {

                        InvResult = response.Content.ReadAsStringAsync().Result;
                        string LRData2 = await response.Content.ReadAsStringAsync();
                        var value2 = JsonConvert.DeserializeObject<CanEwaybillSuccessRoot>(LRData2);
                        if (value2.success == true)
                        {
                            string sqlstr = "update einvoice_generate set EWBDT='" + value2.result.cancelDate + "',STATUS='EWBCAN' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                            InvResult = "1";
                        }

                    }
                }
                return InvResult;
            }
            catch (Exception ex)
            {
                string sqlstr = "update einvoice_generate set ERRORMSG='" + InvResult + "' where DOC_NO='" + dt.Rows[0]["DOC_NO"].ToString() + "'";
                int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                //ErrorLog.WriteLog(ex);
                return InvResult = ex.Message;
            }
        }
    }
}
