using Einv_EwayBill_WebApp.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.ViewModels
{
    public static class OracelDataInsert
    {
        public static void UpdateDataOracle(INVRootObject obj, DataTable dt)
        {
            //return 0;
            try
            {
                if (obj.results.code != 200)
                {
                    string commandText = "UPDATE einvoice_generate_temp SET ERRORMSG=:ERRORMSG " + " WHERE DOC_NO = :DOC_NO";
                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                    {
                        OracleCommand command = new OracleCommand(commandText, connection);

                        // Use AddWithValue to assign Demographics.
                        command.Parameters.Add(":ERRORMSG", OracleDbType.Varchar2);
                        command.Parameters[":ERRORMSG"].Value = obj.results.errorMessage;


                        command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                        command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();

                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                    }


                    //Temprory Use******************************************
                    string commandText1 = "UPDATE Einv_Temp SET ERRORMSG=:ERRORMSG " + " WHERE IDENTIFIER = :DOC_NO";
                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                    {
                        OracleCommand command = new OracleCommand(commandText1, connection);

                        // Use AddWithValue to assign Demographics.
                        command.Parameters.Add(":ERRORMSG", OracleDbType.Varchar2);
                        command.Parameters[":ERRORMSG"].Value = obj.results.errorMessage;


                        command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                        command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();

                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                    }

                    string commandText2 = "UPDATE Einv_Temp1 SET ERRORMSG=:ERRORMSG " + " WHERE IDENTIFIER = :DOC_NO";
                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                    {
                        OracleCommand command = new OracleCommand(commandText2, connection);

                        // Use AddWithValue to assign Demographics.
                        command.Parameters.Add(":ERRORMSG", OracleDbType.Varchar2);
                        command.Parameters[":ERRORMSG"].Value = obj.results.errorMessage;


                        command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                        command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();

                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                    }

                    //End Temprory Use******************************************
                }
                else
                {
                    string commandText = "UPDATE einvoice_generate_temp SET IRN=:IRN,ACKNo=:ACKNo,ACKDATE=:ACKDATE,SIGNEDQRCODE=:SIGNEDQRCODE,SIGNEDINVOICE = :SIGNEDINVOICE ,EWBNO = :EWBNO , EWBDT = :EWBDT,EWBVALIDTILL = :EWBVALIDTILL ,QRCODEURL = :QRCODEURL ,EINVOICEPDF = :EINVOICEPDF  " + " WHERE DOC_NO = :DOC_NO";
                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                    {
                        OracleCommand command = new OracleCommand(commandText, connection);

                        // Use AddWithValue to assign Demographics.
                        command.Parameters.Add(":IRN", OracleDbType.Varchar2);
                        command.Parameters[":IRN"].Value = obj.results.message.Irn;
                        command.Parameters.Add(":ACKNo", OracleDbType.Varchar2);
                        command.Parameters[":ACKNo"].Value = obj.results.message.AckNo;
                        command.Parameters.Add(":ACKDATE", OracleDbType.Varchar2);
                        command.Parameters[":ACKDATE"].Value = obj.results.message.AckDt;
                        command.Parameters.Add(":SIGNEDQRCODE", OracleDbType.Varchar2);
                        command.Parameters[":SIGNEDQRCODE"].Value = obj.results.message.SignedQRCode;
                        command.Parameters.Add(":SIGNEDINVOICE", OracleDbType.Clob);
                        command.Parameters[":SIGNEDINVOICE"].Value = obj.results.message.SignedInvoice;

                        command.Parameters.Add(":EWBNO", OracleDbType.Varchar2);
                        command.Parameters[":EWBNO"].Value = obj.results.message.EwbNo;
                        command.Parameters.Add(":EWBDT", OracleDbType.Varchar2);
                        command.Parameters[":EWBDT"].Value = obj.results.message.EwbDt;
                        command.Parameters.Add(":EWBVALIDTILL", OracleDbType.Varchar2);
                        command.Parameters[":EWBVALIDTILL"].Value = obj.results.message.EwbValidTill;
                        command.Parameters.Add(":QRCODEURL", OracleDbType.Varchar2);
                        command.Parameters[":QRCODEURL"].Value = obj.results.message.QRCodeUrl;
                        command.Parameters.Add(":EINVOICEPDF", OracleDbType.Varchar2);
                        command.Parameters[":EINVOICEPDF"].Value = obj.results.message.EinvoicePdf;

                        command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                        command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();

                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                    }

                    //Temprory Use******************************************
                    string commandText1 = "UPDATE Einv_Temp SET IRN_NO=:IRN " + " WHERE IDENTIFIER = :DOC_NO";
                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                    {
                        OracleCommand command = new OracleCommand(commandText1, connection);

                        // Use AddWithValue to assign Demographics.
                        command.Parameters.Add(":IRN", OracleDbType.Varchar2);
                        command.Parameters[":IRN"].Value = obj.results.message.Irn;


                        command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                        command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();

                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                    }

                    string commandText2 = "UPDATE Einv_Temp1 SET IRN_NO=:IRN " + " WHERE IDENTIFIER = :DOC_NO";
                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                    {
                        OracleCommand command = new OracleCommand(commandText2, connection);

                        // Use AddWithValue to assign Demographics.
                        command.Parameters.Add(":IRN", OracleDbType.Varchar2);
                        command.Parameters[":IRN"].Value = obj.results.message.Irn;


                        command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                        command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();

                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                    }

                    //End Temprory Use******************************************
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteLog(ex);
            }
        }


        public static void InsertDataOracle(INVRootObject obj, DataTable dt)
        {
            //return 0;
            try
            {
                if (obj.results.code != 200)
                {
                    string commandText = "UPDATE einvoice_generate_temp SET ERRORMSG=:ERRORMSG " + " WHERE DOC_NO = :DOC_NO";
                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                    {
                        OracleCommand command = new OracleCommand(commandText, connection);

                        // Use AddWithValue to assign Demographics.
                        command.Parameters.Add(":ERRORMSG", OracleDbType.Varchar2);
                        command.Parameters[":ERRORMSG"].Value = obj.results.errorMessage;


                        command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                        command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();

                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                    }
                }
                else
                {
                    string commandText = "insert into EINVOICE_REF_SETU(GSTIN,IRN,DOC_TYP,DOC_NO,DOC_DT,SIGNEDQRCODE,ACKDATE,ACKNO,SIGNEDINVOICE,EWBNO,EWBDT)values(:GSTIN,:IRN,:DOC_TYP,:DOC_NO,:DOC_DT,:SIGNEDQRCODE,:ACKDATE,:ACKNO,:SIGNEDINVOICE,:EWBNO,:EWBDT)";
                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                    {
                        OracleCommand command = new OracleCommand(commandText, connection);

                        // Use AddWithValue to assign Demographics.
                        command.Parameters.Add(":GSTIN", OracleDbType.Varchar2);
                        command.Parameters[":GSTIN"].Value = dt.Rows[0]["GSTIN"].ToString();
                        command.Parameters.Add(":IRN", OracleDbType.Varchar2);
                        command.Parameters[":IRN"].Value = obj.results.message.Irn;
                        command.Parameters.Add(":DOC_TYP", OracleDbType.Varchar2);
                        command.Parameters[":DOC_TYP"].Value = "GEINV";
                        command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                        command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();
                        command.Parameters.Add(":DOC_DT", OracleDbType.Varchar2);
                        command.Parameters[":DOC_DT"].Value = dt.Rows[0]["DOC_DT"].ToString();
                        command.Parameters.Add(":SIGNEDQRCODE", OracleDbType.Varchar2);
                        command.Parameters[":SIGNEDQRCODE"].Value = obj.results.message.SignedQRCode;
                        command.Parameters.Add(":ACKDATE", OracleDbType.Varchar2);
                        command.Parameters[":ACKDATE"].Value = obj.results.message.AckDt;
                        command.Parameters.Add(":ACKNO", OracleDbType.Varchar2);
                        command.Parameters[":ACKNO"].Value = obj.results.message.AckNo;
                        command.Parameters.Add(":SIGNEDINVOICE", OracleDbType.Clob);
                        command.Parameters[":SIGNEDINVOICE"].Value = obj.results.message.SignedInvoice;
                        command.Parameters.Add(":EWBNO", OracleDbType.Varchar2);
                        command.Parameters[":EWBNO"].Value = obj.results.message.EwbNo;
                        command.Parameters.Add(":EWBDT", OracleDbType.Varchar2);
                        command.Parameters[":EWBDT"].Value = obj.results.message.EwbDt;
                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteLog(ex);
            }
        }
        public static void InsertCancelDataOracle(CancelRoot obj, DataTable dt)
        {
            try
            {
                if (obj.results.status != "200")
                {
                    DateTime date = System.DateTime.Now;
                    string date1 = date.ToString("dd-MM-yyyy HH:mm:ss");
                    string sqlstr = "insert into Finance.EINVOICE_CANCEL(GSTIN, IRN, CNLREM, CNLRSN, STATUS, ERRORMSG, ERRORCODE, CANCELDATE,DOC_NO,DOC_Date) values ('" + dt.Rows[0]["GSTIN"].ToString() + "', '" + dt.Rows[0]["IRN"].ToString() + "', 'Cancel','Not res', '" + obj.results.status + "', '" + obj.results.errorMessage + "', '" + obj.results.code + "', '" + date1 + "', '" + dt.Rows[0]["DOC_NO"].ToString() + "', " + ConvertDateTimeOracleFormat(dt.Rows[0]["Dates"].ToString()) + ")";
                    int SR = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                    // return RVal = "1";
                }
                else
                {
                    string sqlstr = "insert into Finance.EINVOICE_CANCEL(GSTIN, IRN, CNLREM, CNLRSN, STATUS, ERRORMSG, ERRORCODE, CANCELDATE,DOC_NO,DOC_Date) values('" + dt.Rows[0]["GSTIN"].ToString() + "', '" + obj.results.message.Irn + "', '', '', '" + obj.results.status + "', '', '', '" + obj.results.message.CancelDate + "','" + dt.Rows[0]["DOC_NO"].ToString() + "', " + ConvertDateTimeOracleFormat(dt.Rows[0]["Dates"].ToString()) + ")";
                    int SR = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteLog(ex);
            }

        }


        public static void UpdateEWBByIRNNO(SuccessEwbByIrn obj, DataTable dt)
        {
            //return 0;
            try
            {
                if (obj.results.code != 200)
                {
                    string commandText = "UPDATE einvoice_generate_temp SET ERRORMSG=:ERRORMSG " + " WHERE DOC_NO = :DOC_NO";
                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                    {
                        OracleCommand command = new OracleCommand(commandText, connection);

                        // Use AddWithValue to assign Demographics.
                        command.Parameters.Add(":ERRORMSG", OracleDbType.Varchar2);
                        command.Parameters[":ERRORMSG"].Value = obj.results.errorMessage;


                        command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                        command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();

                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                    }
                }
                else
                {
                    string commandText = "UPDATE einvoice_generate_temp SET EWBNO = :EWBNO , EWBDT = :EWBDT,EWBVALIDTILL = :EWBVALIDTILL " + " WHERE DOC_NO = :DOC_NO";
                    using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
                    {
                        OracleCommand command = new OracleCommand(commandText, connection);

                        command.Parameters.Add(":EWBNO", OracleDbType.Varchar2);
                        command.Parameters[":EWBNO"].Value = obj.results.message.EwbNo;
                        command.Parameters.Add(":EWBDT", OracleDbType.Varchar2);
                        command.Parameters[":EWBDT"].Value = obj.results.message.EwbDt;
                        command.Parameters.Add(":EWBVALIDTILL", OracleDbType.Varchar2);
                        command.Parameters[":EWBVALIDTILL"].Value = obj.results.message.EwbValidTill;
                        command.Parameters.Add(":QRCODEURL", OracleDbType.Varchar2);

                        command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                        command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();

                        connection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        //Console.WriteLine("RowsAffected: {0}", rowsAffected);
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteLog(ex);
            }
        }

        public static string ConvertDateTimeOracleFormat(string Dateval)
        {
            if (Dateval != "")
            {
                return "to_date('" + Convert.ToDateTime(Dateval).ToString("MM/dd/yyyy") + "','" + "MM/dd/yyyy" + "')";
            }
            else
                return "''";
        }
    }
}
