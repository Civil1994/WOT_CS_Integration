using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOT_CS.WebAPI.Models;
using WOT_CS.WebAPI.DAL;
using WOT_CS.WebAPI.Services;
using WOT_CS.Core.AppClass;
using WOT_CS.Core.Models;
using WOT_CS.Core.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Globalization;

namespace WOT_CS.WebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class WOTCSIController : ControllerBase
    {
        private readonly Main _objMain; // Variable for Main class
        private readonly ILoggingService _logger;
        public WOTCSIController(ILoggingService logger, IAppSettings appsettings)
        {
            _objMain = new Main(Main.ProcessIntitator.WebAPI, appsettings);
            _logger = logger;
        }

      
        [HttpGet("GetEmployee")]
        public IActionResult GetEmployee(string UniqueEmployeeId=null,DateTime? ModifiedDate=null,string Status=null,
        int page = 1,
        int pageSize = 50)
        {
            try
            {

                if (!string.IsNullOrEmpty(UniqueEmployeeId)) 
                {
                    bool exist = _objMain.IsExist("Employee", UniqueEmployeeId, "EmpCode");

                    if (!exist)
                    {
                        return BadRequest(new
                        {
                            success = false,
                            message = "Invalid EmployeeID",
                            data = "Employee does not exist"
                        });
                    }
                }

                var list = _objMain.GetEmployeeDetails(UniqueEmployeeId, ModifiedDate, Status);

                int totalRecords = list.Count;

                var pagedData = list
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    success = true,
                    page,
                    pageSize,
                    totalRecords,
                    totalPages = (int)Math.Ceiling(
                        (double)totalRecords / pageSize
                    ),
                    data = pagedData
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetEmployee failed");

                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost("SaveShiftPlanning")]
            public IActionResult SaveShiftPlanning([FromBody] List<ShiftPlanningModel> planningmodel)
        {
            if (planningmodel == null || planningmodel.Count == 0)
            {
                var error = new Response
                {
                    Status = 0,
                    Message = "Shift planning data is required.",
                    Data = new ResponseData
                    {
                        ErrorData = { "Shift planning data cannot be empty." }
                    }
                };

                return BadRequest(error);
            }

            // Validate every record
            foreach (var item in planningmodel)
            {
                if (string.IsNullOrWhiteSpace(item.EmpCode))
                {
                    var error = new Response
                    {
                        Status = 0,
                        Message = "employee_code is required.",
                        Data = new ResponseData
                        {
                            ErrorData = { "employee_code cannot be empty." }
                        }
                    };

                    return BadRequest(error);
                }
            }


            try
            {
                string responseMsg = "Shift Planning Saved Successfully.";
                ResponseData rd = new ResponseData();
                int sts = 1;
                int wotiProcessId = 0;

                _objMain.SaveShiftPlanning(planningmodel, out wotiProcessId);

                if (wotiProcessId != 0)
                {
                    DataTable dtlog = _objMain.GetProcessLog(wotiProcessId);
                    DataTable dterrors = _objMain.GetProcessError(wotiProcessId);

                    if (dtlog.Rows.Count > 0)
                    {
                        responseMsg = dtlog.Rows[0]["Remarks"].ToString();
                    }
                  

                    if (dterrors.Rows.Count > 0)
                    {
                        foreach (DataRow drow in dterrors.Rows)
                        {
                            rd.ErrorData.Add(drow["ErrorText"].ToString());
                        }
                    }
                }


                //Verify Data.
                var success = new Response
                {
                    Status = sts,
                    Message = responseMsg,
                    Data = rd
                };

                return Ok(success);
            }
            catch (ManualException ex)
            {
                var error = new Response
                {
                    Status = 0,
                    Message = "Error saving Shift Planning",
                    Data = new ResponseData { ErrorData = { string.IsNullOrEmpty(ex.Message) ? "internal server error" : ex.Message } }
                };
                return StatusCode(500, error);
            }
            catch (Exception ex)
            {
                var error = new Response
                {
                    Status = 0,
                    Message = "Error Shift Planning",
                    Data = new ResponseData { ErrorData = { "unhandled error occured" } }
                };
                return StatusCode(500, error);
            }
        }
          
    }
}
