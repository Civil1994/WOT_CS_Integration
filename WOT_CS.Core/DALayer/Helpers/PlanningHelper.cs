using WOT_CS.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOT_CS.Core.AppClass;

namespace WOT_CS.Core.DALayer.Helpers
{
    public class PlanningHelper
    {
        public static void AddWOTShiftPlanning(ShiftPlanningModel planning, int wotiprocessid)
        {
            try
            {
                Common.Log("INFO: AddWOTShiftPlanning Started");

                string insertQuery = @"
        INSERT INTO WOT_CSI_Employee
        (
            WOTProcessId,ProcessDate,PersonnelNo,CheckInId, FirstName,LastName,Agency,Status, Department, RoleName, Title, JobBand, Casual, StartDay,EndDay
        )
        VALUES
        (
            @WOTProcessId,@ProcessDate,@PersonnelNo,  @CheckInId,@FirstName,@LastName, @Agency,
            @Status, @Department, @RoleName,@Title, @JobBand, @Casual,@StartDay, @EndDay
        )";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@WOTProcessId", wotiprocessid },
             { "@ProcessDate", DateTime.Now },
            { "@PlanningDate", planning.Date },
            { "@EmpID", planning.EmpCode },
            { "@StartTime1", (object)planning.StartTime1 ?? DBNull.Value },
            { "@EndTime1", (object)planning.EndTime1 ?? DBNull.Value },
            { "@StartTime2", (object)planning.StartTime2 ?? DBNull.Value },
            { "@EndTime2", (object)planning.EndTime2 ?? DBNull.Value }
        };

                ConnectionFunctions.ExecuteScalar(insertQuery, parameters);

                Common.Log("INFO: Shift Planning data inserted successfully");
            }
            catch (Exception ex)
            {
                Common.Log("ERROR: AddWOTShiftPlanning - " + ex.Message);
            }


        }

        public static void AddWOTShiftPlanninge(List<ShiftPlanningModel> planningList,int wotiprocessid)
        {
            try
            {
                Common.Log("INFO: AddWOTShiftPlanning Started");

                if (planningList == null || planningList.Count == 0)
                {
                    Common.Log("INFO: No Shift Planning data to insert.");
                    return;
                }

                string insertQuery = @"
            INSERT INTO WOT_CSI_ShiftPlanning
            (
                WOTProcessId,
                ProcessDate,
                PlanningDate,
                EmpID,
                StartTime1,
                EndTime1,
                StartTime2,
                EndTime2
            )
            VALUES
            (
                SELECT
                @WOTProcessId,
                @ProcessDate,
                @PlanningDate,
                E.EmpID,
                @StartTime1,
                @EndTime1,
                @StartTime2,
                @EndTime2

            FROM Employee E
            WHERE LTRIM(RTRIM(E.EmpCode)) = LTRIM(RTRIM(@EmpCode)
            )";

                foreach (ShiftPlanningModel planning in planningList)
                {
                    Dictionary<string, object> parameters =
                        new Dictionary<string, object>
                    {
                { "@WOTProcessId", wotiprocessid },
                { "@ProcessDate", DateTime.Now },

                { "@PlanningDate",
                    (object)planning.Date ?? DBNull.Value },

                //{ "@EmpCode",
                //    (object)planning.EmpCode ?? DBNull.Value },

                { "@StartTime1",
                    (object)planning.StartTime1 ?? DBNull.Value },

                { "@EndTime1",
                    (object)planning.EndTime1 ?? DBNull.Value },

                { "@StartTime2",
                    (object)planning.StartTime2 ?? DBNull.Value },

                { "@EndTime2",
                    (object)planning.EndTime2 ?? DBNull.Value }
                    };

                    ConnectionFunctions.ExecuteScalar(
                        insertQuery,
                        parameters);
                }

                Common.Log(
                    "INFO: Shift Planning data inserted successfully. " +
                    "Records: " + planningList.Count);
            }
            catch (Exception ex)
            {
                Common.Log(
                    "ERROR: AddWOTShiftPlanning - " +
                    ex.Message +
                    Environment.NewLine +
                    ex.StackTrace);
            }
        }
        public static void AddWOTShiftPlanning( List<ShiftPlanningModel> planningList,int wotiprocessid)
        {
            try
            {
                Common.Log("INFO: AddWOTShiftPlanning Started");

                if (planningList == null || planningList.Count == 0)
                {
                    Common.Log("INFO: No Shift Planning data to insert.");
                    return;
                }

                string empIdQuery = @"
            SELECT EmpID
            FROM Employee
            WHERE LTRIM(RTRIM(EmpCode)) = LTRIM(RTRIM(@EmpCode))";

                string insertQuery = @"
            INSERT INTO WOT_CSI_ShiftPlanning
            (
                WOTProcessId,
                ProcessDate,
                PlanningDate,
                EmpID,
                StartTime1,
                EndTime1,
                StartTime2,
                EndTime2
            )
            VALUES
            (
                @WOTProcessId,
                @ProcessDate,
                @PlanningDate,
                @EmpID,
                @StartTime1,
                @EndTime1,
                @StartTime2,
                @EndTime2
            )";

                foreach (ShiftPlanningModel planning in planningList)
                {
                    if (string.IsNullOrWhiteSpace(planning.EmpCode))
                    {
                        string errorMessage = "EmpCode is null or empty.";

                        Common.Log(
                            "ERROR: " + errorMessage);

                        Common.LogWOTErrorLog(
                            wotiprocessid,
                            planning.EmpCode ?? "",
                            "AddWOTShiftPlanning",
                            errorMessage);

                        continue;
                    }
                    // Get EmpID using EmpCode from the model
                    Dictionary<string, object> empParameters =
                        new Dictionary<string, object>
                        {
                    {
                        "@EmpCode",
                        (object)planning.EmpCode ?? DBNull.Value
                    }
                        };

                    object empIdResult = ConnectionFunctions.ExecuteScalar(
                        empIdQuery,
                        empParameters);
                   
                    if (empIdResult == null || empIdResult == DBNull.Value)
                    {
                        Common.Log( "ERROR: Employee not found for EmpCode: [" + planning.EmpCode + "]");
                        Common.LogWOTErrorLog(wotiprocessid, planning.EmpCode, "AddWOTShiftPlanninge ", "ERROR: Employee not found for EmpCode: [" +
                            planning.EmpCode + "]");
                       
                        continue;
                    }

                    int empId = Convert.ToInt32(empIdResult);

                    Common.Log( "INFO: EmpCode [" +  planning.EmpCode + "] mapped to EmpID [" +  empId + "]");

                    // Insert using actual EmpID
                    Dictionary<string, object> parameters =
                        new Dictionary<string, object>
                        {
                    { "@WOTProcessId", wotiprocessid },

                    { "@ProcessDate", DateTime.Now },

                    { "@PlanningDate",
                        (object)planning.Date ?? DBNull.Value },

                    { "@EmpID", empId },

                    { "@StartTime1",
                        (object)planning.StartTime1 ?? DBNull.Value },

                    { "@EndTime1",
                        (object)planning.EndTime1 ?? DBNull.Value },

                    { "@StartTime2",
                        (object)planning.StartTime2 ?? DBNull.Value },

                    { "@EndTime2",
                        (object)planning.EndTime2 ?? DBNull.Value }
                        };

                    ConnectionFunctions.ExecuteScalar(
                        insertQuery,
                        parameters);
                }

                Common.Log(
                    "INFO: Shift Planning data inserted successfully. " +
                    "Records: " + planningList.Count);
            }
            catch (Exception ex)
            {
                Common.Log(
                    "ERROR: AddWOTShiftPlanning - " +
                    ex.Message +
                    Environment.NewLine +
                    ex.StackTrace);
                Common.LogWOTErrorLog(wotiprocessid,  "", "AddWOTShiftPlanning trycatch block", ex.Message);
                throw;
            }
        }

    }
}
