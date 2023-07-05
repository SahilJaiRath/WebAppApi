using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Einv_EwayBill_WebApp.Models
{
    public class ClsDynamic
    {
        public static string GenerateUniqueNumber()
        {
            long j = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                j *= ((int)b + 1);
            }
            string finalString = string.Format("{0:x}", j - DateTime.Now.Ticks);
            return finalString;
        }

        public static void DeleteFiles(string dirName)
        {
            //string logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"ErrorLogs\\");
           
            string[] files = Directory.GetFiles(dirName);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastWriteTime < DateTime.Now.AddDays(-2))
                {
                    fi.Delete();
                }
                else if (fi.LastAccessTime < DateTime.Now.AddDays(-2))
                {
                    fi.Delete();
                }
            }
        }

        public static void WriteLog(string ex)
        {

            string logFilePath = string.Empty;
            string logFile = string.Empty;

            //Get file location and generate file name.
            Guid g;
            g = Guid.NewGuid();
            logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ErrorLogs");
            //logFilePath = "D:\\MawaiMail\\";    
            logFile = logFilePath + "\\" + g + ".txt";
            var delpath = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLogs");
            DeleteFiles(delpath);
            //Write exception to file.
            FileStream fs1 = new FileStream(logFile, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs1);
            writer.WriteLine("Error Date :" + DateTime.Now.ToString());
            writer.WriteLine("Exception :" + ex.ToString());
            writer.Close();

        }

        public static void WriteLog(string ex, string DocName)
        {
            string logFilePath = string.Empty;
            string logFile = string.Empty;

            //Get file location and generate file name.
            Guid g;
            g = Guid.NewGuid();
            logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ErrorLogs");
            var delpath = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLogs");
            DeleteFiles(delpath);
            logFile = logFilePath + "\\" + DocName + ".txt";

            if (!File.Exists(logFile))
            {
                // Creating the same file if it doesn't exist 
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine("Error Date :" + DateTime.Now.ToString());
                    sw.WriteLine("Exception :" + ex.ToString());
                }
            }
            else
            {
                // Appending the given texts 
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine("Error Date :" + DateTime.Now.ToString());
                    sw.WriteLine("Exception :" + ex.ToString());
                }
            }

        }

        public static void JsonLog(string ex, string DocName)
        {
            string logFilePath = string.Empty;
            string logFile = string.Empty;

            //Get file location and generate file name.
            Guid g;
            g = Guid.NewGuid();
            logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JsonLogs");
            var delpath = Path.Combine(Directory.GetCurrentDirectory(), "JsonLogs");
            DeleteFiles(delpath);
            logFile = logFilePath + "\\" + DocName + ".txt";

            if (!File.Exists(logFile))
            {
                // Creating the same file if it doesn't exist 
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine("Error Date :" + DateTime.Now.ToString());
                    sw.WriteLine("Exception :" + ex.ToString());
                }
            }
            else
            {
                // Appending the given texts 
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine("Error Date :" + DateTime.Now.ToString());
                    sw.WriteLine("Exception :" + ex.ToString());
                }
            }

        }

        public static string UpdateErrorLog(string Errordata, string DocNo)
        {
            string Rval = "";
            string DQuote = @"'";
            string sqlstr = "update einvoice_generate_temp set ERRORMSG='" + Errordata.Replace(DQuote, "") + "' where DOC_NO='" + DocNo + "'";
            int i = DataLayer.ExecuteNonQuery(OraDBConnection.OrclConnection, CommandType.Text, sqlstr);
            return Rval = i.ToString();
        }
    }
}
