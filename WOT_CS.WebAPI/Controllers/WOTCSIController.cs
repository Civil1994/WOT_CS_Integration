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
        public IActionResult GetEmployee(string UniqueEmployeeId=null,DateTime? ModifiedBy=null,string Status=null,
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

                var list = _objMain.GetEmployeeDetails(UniqueEmployeeId, ModifiedBy, Status);

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

      
    }
}
