using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    public class Taxilla_OracelDataInsert
    {
        public static void UpdateDataOracle(successRoot obj, DataTable dt)
        {
            string commandText = "UPDATE einvoice_generate_temp SET IRN=:IRN,ACKNo=:ACKNo,ACKDATE=:ACKDATE,SIGNEDQRCODE=:SIGNEDQRCODE,SIGNEDINVOICE = :SIGNEDINVOICE ,EWBNO = :EWBNO , EWBDT = :EWBDT,EWBVALIDTILL = :EWBVALIDTILL ,QRCODEURL = :QRCODEURL ,EINVOICEPDF = :EINVOICEPDF  " + " WHERE DOC_NO = :DOC_NO";
            using (OracleConnection connection = new OracleConnection(OraDBConnection.OrclConnection))
            {
                OracleCommand command = new OracleCommand(commandText, connection);

                // Use AddWithValue to assign Demographics.
                command.Parameters.Add(":IRN", OracleDbType.Varchar2);
                command.Parameters[":IRN"].Value = obj.result.Irn;
                command.Parameters.Add(":ACKNo", OracleDbType.Varchar2);
                command.Parameters[":ACKNo"].Value = obj.result.AckNo;
                command.Parameters.Add(":ACKDATE", OracleDbType.Varchar2);
                command.Parameters[":ACKDATE"].Value = obj.result.AckDt;
                command.Parameters.Add(":SIGNEDQRCODE", OracleDbType.Varchar2);
                command.Parameters[":SIGNEDQRCODE"].Value = obj.result.SignedQRCode;
                command.Parameters.Add(":SIGNEDINVOICE", OracleDbType.Clob);
                command.Parameters[":SIGNEDINVOICE"].Value = obj.result.SignedInvoice;

                command.Parameters.Add(":EWBNO", OracleDbType.Varchar2);
                command.Parameters[":EWBNO"].Value = obj.result.EwbNo;
                command.Parameters.Add(":EWBDT", OracleDbType.Varchar2);
                command.Parameters[":EWBDT"].Value = obj.result.EwbDt;
                command.Parameters.Add(":EWBVALIDTILL", OracleDbType.Varchar2);
                command.Parameters[":EWBVALIDTILL"].Value = obj.result.EwbValidTill;
                command.Parameters.Add(":QRCODEURL", OracleDbType.Varchar2);
                command.Parameters[":QRCODEURL"].Value = "";
                command.Parameters.Add(":EINVOICEPDF", OracleDbType.Varchar2);
                command.Parameters[":EINVOICEPDF"].Value = "";

                command.Parameters.Add(":DOC_NO", OracleDbType.Varchar2);
                command.Parameters[":DOC_NO"].Value = dt.Rows[0]["DOC_No"].ToString();

                connection.Open();
                Int32 rowsAffected = command.ExecuteNonQuery();
                //Console.WriteLine("RowsAffected: {0}", rowsAffected);
            }
        }
    }
}
