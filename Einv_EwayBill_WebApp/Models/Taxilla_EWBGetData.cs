using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    public class Taxilla_EWBGetData
    {
        public static DataTable GetDistinctData(string EINVType, string SessionID)
        {
            DataTable dt = new DataTable();
            string sqlstr = "";
            if (EINVType == "GEWB")
            {
                sqlstr = "select distinct DOC_NO,CDKEY,EINVPASSWORD,EINVUSERNAME,EFUSERNAME,EFPASSWORD,IRN,EWBNO from einvoice_generate_temp where ID = '" + SessionID + "'";
                //sqlstr = "select DISTINCT ID,DOCNO,EFUSERNAME,EFPASSWORD,EINVUSERNAME,EINVPASSWORD  from EWB_GEN_STD where ID='" + SessionID + "' ";
            }
            if (EINVType == "CEWB")
            {
                sqlstr = "select DISTINCT DOC_NO,EINVPASSWORD,EINVUSERNAME,EFUSERNAME,EFPASSWORD,EWBNO from einvoice_generate where DOC_No='" + SessionID + "' and status is null";
            }
            if (EINVType == "NUSER")
            {
                sqlstr = "select distinct EWAY_USERNAME,EWAY_PASSWORD,EWAY_CLIENT_ID,EWAY_CLIENT_SECRET,EWAYBILL_GST,EWAY_CLIENT_USERNAME,EWAY_CLIENT_PASSWORD  from unit ";
            }
            if (EINVType == "BULK_EWB")
            {
                sqlstr = "select distinct EWAY_USERNAME,EWAY_PASSWORD,EWAY_CLIENT_ID,EWAY_CLIENT_SECRET,EWAYBILL_GST,EWAY_CLIENT_USERNAME,EWAY_CLIENT_PASSWORD  from unit ";
            }

            dt = DataLayer.ExecuteDataset(OraDBConnection.OrclConnection, CommandType.Text, sqlstr).Tables[0];
            return dt;

        }

        public static DataTable GetEWBData(string EINVType, string SessionID, string Doc_No)
        {
            DataTable dt = new DataTable();
            string sqlstr = "";
            if (EINVType == "GEWB")
            {
                //sqlstr = "select * from EWB_GEN_STD where ID='" + SessionID + "' and DOCNO='" + Doc_No + "'";

                sqlstr = "select * from einvoice_generate_temp where ID='" + SessionID + "' and DOC_NO='" + Doc_No + "'";
            }
            if (EINVType == "CEWB")
            {
                sqlstr = "select * from einvoice_generate where EWBNO='" + SessionID + "'";
            }
            if (EINVType == "UPD_V_NO")
            {
                sqlstr = "select * from EWB_GEN_STD where EWAY_BILL_NO='" + SessionID + "'";
            }
            if (EINVType == "CONSO_EWB")
            {
                sqlstr = "select * from EWB_GEN_STD where EWAY_BILL_NO='" + SessionID + "'";
            }
            if (EINVType == "REJ_EWB")
            {
                sqlstr = "select * from EWB_GEN_STD where EWAY_BILL_NO='" + SessionID + "'";
            }
            if (EINVType == "GET_EWB")
            {
                sqlstr = "select * from EWB_GEN_STD where EWAY_BILL_NO='" + SessionID + "'";
            }
            if (EINVType == "BULK_EWB")
            {
                sqlstr = "select * from EWB_GEN_STD where EWAY_BILL_NO='" + SessionID + "'";
            }


            dt = DataLayer.ExecuteDataset(OraDBConnection.OrclConnection, CommandType.Text, sqlstr).Tables[0];
            return dt;

        }
    }
}
