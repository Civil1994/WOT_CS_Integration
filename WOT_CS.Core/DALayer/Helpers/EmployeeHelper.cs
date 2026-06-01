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
    public class EmployeeHelper
    {

        public static List<EmployeeModel> GetEmployee(string UniqueEmployeeId = null, DateTime? ModifiedBy = null, string Status = null)
        {
            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = ConnectionFunctions.GetConnectionString();

            List<EmployeeModel> employees =
                new List<EmployeeModel>();
            Common.Log("INFO: GetEmployee Started");
            try
            {
                myConn.Open();

                string sqry = "WOT_CSI_GetEmployee";

                SqlCommand myCmd = new SqlCommand(sqry, myConn);

                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.Parameters.AddWithValue("@UniqueEmployeeId",
                    (object)UniqueEmployeeId ?? DBNull.Value);

                myCmd.Parameters.AddWithValue("@ModifiedBy",
                    (object)ModifiedBy ?? DBNull.Value);

                myCmd.Parameters.AddWithValue("@Status",
                    (object)Status ?? DBNull.Value);


                using (SqlDataReader dataReader = myCmd.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            EmployeeModel oEmployee =
                                new EmployeeModel();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("PersonnelNo")))
                                oEmployee.PersonnelNo =
                                    dataReader["PersonnelNo"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("CheckInId")))
                                oEmployee.CheckInId =
                                    dataReader["CheckInId"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("FirstName")))
                                oEmployee.FirstName =
                                    dataReader["FirstName"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastName")))
                                oEmployee.LastName =
                                    dataReader["LastName"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Agency")))
                                oEmployee.Agency =
                                    dataReader["Agency"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Status")))
                                oEmployee.Status =
                                    dataReader["Status"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Department")))
                                oEmployee.Department =
                                    dataReader["Department"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("RoleName")))
                                oEmployee.RoleName =
                                    dataReader["RoleName"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Title")))
                                oEmployee.Title =
                                    dataReader["Title"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("JobBand")))
                                oEmployee.JobBand =
                                      dataReader["JobBand"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("Casual")))
                                oEmployee.Casual =
                                    dataReader["Casual"].ToString();

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("StartDay")))
                                oEmployee.StartDay =
                                    Convert.ToDateTime(dataReader["StartDay"]);

                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("EndDay")))
                                oEmployee.EndDay =
                                    Convert.ToDateTime(dataReader["EndDay"]);

                            // ADD TO LIST
                            employees.Add(oEmployee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log(
                   "ERROR: GetEmployee " + ex.Message);
            }
            finally
            {
                if (myConn.State != ConnectionState.Closed)
                    myConn.Close();
                Common.Log("INFO: GetEmployee Completed");
            }

            return employees;
        }

        public static void AddWOTEmployeeData(EmployeeModel employee, int wotiprocessid)
        {
            try
            {
                Common.Log("INFO: AddWOTEmployeeData Started");

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
            { "@PersonnelNo", employee.PersonnelNo },
            { "@CheckInId", employee.CheckInId },
            { "@FirstName", employee.FirstName },
            { "@LastName", employee.LastName },
            { "@Agency", employee.Agency },
            { "@Status", employee.Status },
            { "@Department", employee.Department },
            { "@RoleName", employee.RoleName },
            { "@Title", employee.Title },
            { "@JobBand", employee.JobBand },
            { "@Casual", employee.Casual },
            { "@StartDay", employee.StartDay },
            { "@EndDay", employee.EndDay }
        };

                ConnectionFunctions.ExecuteScalar(insertQuery, parameters);

                Common.Log("INFO: Employee data inserted successfully");
            }
            catch (Exception ex)
            {
                Common.Log("ERROR: AddWOTEmployeeData - " + ex.Message);
            }

          
        }
    }
}
