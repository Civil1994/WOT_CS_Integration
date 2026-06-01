using WOT_CS.Core.BL;
using WOT_CS.Core.DALayer;
using WOT_CS.Core.Enums;
using WOT_CS.Core.Models;
using WOT_CS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WOT_CS.Core.APIClient;
using Newtonsoft.Json;
using WOT_CS.Core.Configuration;
using WOT_CS.Core.DALayer.Helpers;

namespace WOT_CS.Core.AppClass
{
    public class Main
    {
        //private List<LocalFile> LocalFiles = new List<LocalFile>();
        public Dictionary<string, string> tempFolders;// List of folders to process files from
        public Dictionary<string, string> tempOutputFolders;// List of folders for output
        public string appFilesPath;
        public string tempDirectory;



        ProcessIntitator _caller;

        //EmployeeExportBL empexportbl;
        //LeaveExportBL leaveexportbl;
        //DailyHoursImportBL dhimportbl;



        public enum Msgtype
        {
            Error = 1,
            Success = 2,
            Info = 3,
            Warning = 4,
        }
        public enum ProcessIntitator
        {
            BackroudWindowService = 1,
            WindowsForm = 2,
            WebAPI = 3,
        }

        public Main()
        {
        }
        public Main(ProcessIntitator pi, IAppSettings appSettings)
        {
            _caller = pi;
            //empexportbl = new EmployeeExportBL();
            //SFTPService sFTPService = new SFTPService();
            //leaveexportbl = new LeaveExportBL(sFTPService);
            //dhimportbl = new DailyHoursImportBL(sFTPService);

            ConnectionFunctions.Initialize(appSettings.ConnectionString);

        }

        

        public List<EmployeeModel> GetEmployeeDetails(string UniqueEmployeeId = null, DateTime? ModifiedBy = null, string Status = null)
        {
            int wotiProcessId = 0;

            List<EmployeeModel> emp =
                new List<EmployeeModel>();

            try
            {
                Common.Log("INFO: GetEmployeeDetails Started");

                wotiProcessId =
                    Common.CreateWOTProcessLogEntry( "Get Employee Details" );

                emp = EmployeeHelper.GetEmployee(UniqueEmployeeId, ModifiedBy, Status);
                foreach (var employee in emp)
                {
                    EmployeeHelper.AddWOTEmployeeData(employee, wotiProcessId);
                }
              

                Common.UpdateWOTProcessLogEntry(wotiProcessId, 0, "Employee fetched successfully" );

                Common.Log( "INFO: GetEmployeeDetails Completed" );
            }
            catch (Exception ex)
            {
                Common.UpdateWOTProcessLogEntry(wotiProcessId, 1, ex.Message);

                Common.Log(
                    "ERROR: GetEmployeeDetails " + ex.Message
                );
            }

            return emp;
        }

        public bool IsExist(string TableName, string Value, string filter)
        {
            bool Exist = Common.IsExist(TableName, Value, filter);
            return Exist;
        }

    }
}
