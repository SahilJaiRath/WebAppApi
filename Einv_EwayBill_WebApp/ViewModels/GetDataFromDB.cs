using Einv_EwayBill_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.ViewModels
{
    public class GetDataFromDB
    {
        public static DataTable GetDistinctData(string EINVType, string SessionID,string Eitype)
        {
            DataTable dt = new DataTable();
            string sqlstr = "";
            
            if (EINVType == "APPTYPE")
            {
                sqlstr = "select '"+ Eitype + "' as value_1 from dual";
            }

            if (EINVType == "CHECKINVEWB")
            {
                sqlstr = "select ITEM_BARCDE,BILLFROM_GSTIN,BILLTO_GSTIN from einvoice_generate_temp where ID='" + SessionID + "'";
            }

            if (EINVType == "GETID")
            {
                sqlstr = "select distinct ID,DOC_NO,CDKEY,EINVPASSWORD,EINVUSERNAME,EFUSERNAME,EFPASSWORD,IRN,EWBNO,ITEM_BARCDE,BILLFROM_GSTIN,BILLTO_GSTIN from einvoice_generate_temp where DOC_NO='" + SessionID + "'";
            }

            if (EINVType == "GINV")
            {
                sqlstr = "select distinct DOC_NO,CDKEY,EINVPASSWORD,EINVUSERNAME,EFUSERNAME,EFPASSWORD,IRN,EWBNO from einvoice_generate_temp where ID='" + SessionID + "' ";
            }
            if (EINVType == "CINV")
            {
                //sqlstr = "select INV_BILL_NO as IRN,IDEntifier as DOC_NO,dates,PUBLICKEY as GSTIN, EINVUSERNAME, EINVPASSWORD, CLIENT_ID as EFUSERNAME, CLIENT_SECRET as EFPASSWORD from invoice_header, UNIT where CODE = UNIT_CODE and INV_BILL_NO = '" + SessionID + "' UNION select IRN_NO as IRN,nvl(DN_VOU_NO, to_char(DN_CN_NO)) as DOC_NO,DN_CN_DT dates, PUBLICKEY as GSTIN, EINVUSERNAME, EINVPASSWORD, CLIENT_ID as EFUSERNAME, CLIENT_SECRET as EFPASSWORD from DR_CR_NOTE, UNIT where CODE = DN_CN_UNIT and IRN_NO = '" + SessionID + "' ";
                sqlstr = "select distinct DOC_NO,EINVPASSWORD,EINVUSERNAME,EFUSERNAME,EFPASSWORD,IRN,GSTIN,EWBNO from einvoice_generate where DOC_NO = '" + SessionID+ "' and IRN is not null and status is null";
            }
            if (EINVType == "DTLINV")
            {
                sqlstr = "select distinct DOC_NO,EINVPASSWORD,EINVUSERNAME,EFUSERNAME,EFPASSWORD from einvoice_generate_temp where IRN='" + SessionID + "' ";
            }
            if (EINVType == "EWBBYIRN")
            {
                sqlstr = "select distinct DOC_NO,EINVPASSWORD,EINVUSERNAME,EFUSERNAME,EFPASSWORD,IRN from einvoice_generate_temp where DOC_NO='" + SessionID + "' ";
            }
            if (EINVType == "BULKINV")
            {
                sqlstr = "select distinct DOC_NO,EINVPASSWORD,EINVUSERNAME,EFUSERNAME,EFPASSWORD from einvoice_generate_temp where IRN='" + SessionID + "' ";
            }

            dt = DataLayer.ExecuteDataset(OraDBConnection.OrclConnection, CommandType.Text, sqlstr).Tables[0];
            return dt;

        }

        public static DataTable GetEinvoiceData(string EINVType, string SessionID, string Doc_No)
        {
            DataTable dt = new DataTable();
            string sqlstr = "";
            if (EINVType == "GINV")
            {
                sqlstr = "select * from einvoice_generate_temp where id='"+ SessionID + "' and DOC_No='" + Doc_No + "'";
            }
            if (EINVType == "CINV" || EINVType == "EWBBYIRN")
            {
                //sqlstr = "select * from einvoice_generate_temp where ID='" + SessionID + "' and DOC_No='" + Doc_No + "' and IRN is not null";
                sqlstr = "select * from einvoice_generate where DOC_No='" + Doc_No + "' and IRN is not null";
            }
            if (EINVType == "DTLINV")
            {
                sqlstr = "select * from einvoice_generate_temp where IRN='" + SessionID + "'";
            }

            if (EINVType == "BULKINV")
            {
                sqlstr = "select * from einvoice_generate_temp where DOC_No='" + Doc_No + "' and IRN is null";
            }
            dt = DataLayer.ExecuteDataset(OraDBConnection.OrclConnection, CommandType.Text, sqlstr).Tables[0];
            return dt;

        }
    }
}
