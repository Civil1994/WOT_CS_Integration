using WOT_CS.Core.DALayer;
using WOT_CS.Core.HCMS.Entity;
using WOT_CS.Core.Models;
using WOT_CS.Core.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WOT_CS.Core.AppClass
{
    public class Common
    {
        //Note: some constant values specific to DBS, need to make it general
        public const string strSvcUserId = "AUTO";
        public const int nSvcUserNo = 3;
        public const string Language = "0"; //english
        public const string AllowFuturePosting_TA = "0"; //SELECT ISNULL(CONVERT(SMALLINT,Val),0) 'Val' FROM MasterSetup WHERE Code = 23
        public const string CompanyName = "SME";//SELECT ISNULL(Val,'') AS Vals FROM MasterSetup WHERE Code = 15
        public const string HierarchyLevel = "5";//SELECT ISNULL(Val,'') AS Vals FROM MasterSetup WHERE Code = 15
        public static readonly string[] UserInfo = new string[] { nSvcUserNo.ToString(), strSvcUserId, "", "", HierarchyLevel, "", "", "", "", "", "", "", AllowFuturePosting_TA, Language, CompanyName, "", "", "", "" };

        static int iResult = 0;
        static string errmsg = "";
        static string appFilesPath { get => ConfigurationManager.AppSettings["AppFilesPath"]; }

        public const string logFileName = "Log.txt";
        public const string exceptionFilePath = "ExceptionLog.txt";

        public static Hashtable ModulesTable;
        internal static int CreateWOTProcessLogEntry(string processName)
        {
            int dboxiprocessid = 0;
            try
            {

                Log("INFO: CreateWOTProcessLogEntry Started");


                string errorQuery = " INSERT INTO WOTProcessLog ([ProcessName],[StartTime]) VALUES (@ProcessName, GETDATE());    ";
                errorQuery += " SELECT SCOPE_IDENTITY() AS LastInsertedId;";

                Dictionary<string, object> parameters = new Dictionary<string, object>    {
                { "@ProcessName", processName },
            };

                string errorMsg = string.Empty;
                object result = ConnectionFunctions.ExecuteScalar(errorQuery, parameters);

                if (result != null)
                {
                    dboxiprocessid = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Log(
                    "ERROR: CreateWOTProcessLogEntry " + ex.Message
                );
            }
            return dboxiprocessid;


        }
        internal static int UpdateWOTProcessLogEntry(
        int wotiProcessId,
        int hasErrors,
        string remarks)
        {
            Log("INFO: UpdateWOTProcessLogEntry Started");
            int rowsAffected = 0;

            string query = @"
        UPDATE WOTProcessLog  SET  EndTime = @EndTime,HasErrors = @HasErrors,
            Remarks = @Remarks WHERE WOTProcessId = @WOTProcessId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@EndTime", DateTime.Now },  { "@HasErrors", hasErrors }, { "@Remarks", remarks },{ "@WOTProcessId", wotiProcessId }
    };

            string errorMsg = string.Empty;

            object result = ConnectionFunctions.ExecuteQuery(
                query,
                parameters,
                ref errorMsg
            );

            if (result != null)
            {
                rowsAffected = Convert.ToInt32(result);
            }

            return rowsAffected;
        }
        public static void Log(string message)
        {
            string logFolder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Logs"
            );

            // Create folder if not exists
            Directory.CreateDirectory(logFolder);

            // File name based on current date
            string fileName = $"api_log_{DateTime.Now:yyyyMMdd}.txt";

            string path = Path.Combine(logFolder, fileName);

            File.AppendAllText(
                path,
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}"
            );
        }

        public static bool IsExist(string TableName, string Value, string Filter)
        {
            string errmsg = "";
            bool RetVal = false;
            string strCode = "";
            int fil = 0;
            if (ConnectionFunctions.Connect_SQLScalar(ref fil, "select 1 from " + TableName + " where  " + Filter + "= '" + Value + "'", ref errmsg))
            {
                if (fil > 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }


    }
}
