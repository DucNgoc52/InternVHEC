using IMS_LEARN.Common;
using IMS_LEARN.Models;
using IMS_LEARN.Services.SvStaff;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic;

namespace IMS_LEARN.Controllers
{
    [ApiController]
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost]
        [Route("GetList")]
        public IActionResult GetList(ResquestParameters parameters)
        {
            var result = new List<StaffModel>();
            try
            {
                var tempStaffs = _staffService.GetList();
                
                // Search
                if (!string.IsNullOrEmpty(parameters.Filter))
                {
                    var optionAssembly = ScriptOptions.Default.AddReferences(typeof(StaffModel).Assembly);
                    var tempFilterExpression = CSharpScript.EvaluateAsync<Func<StaffModel, bool>>($"s=> {parameters.Filter}", optionAssembly);
                    Func<StaffModel, bool> filterExpression = tempFilterExpression.Result;

                    tempStaffs = tempStaffs.Where(filterExpression).AsQueryable();
                    //result = tempData.ToList();
                }

                // Order by
                if (!string.IsNullOrEmpty(parameters.OrderBy))
                {
                    tempStaffs = tempStaffs.OrderBy(parameters.OrderBy);
                }
                else
                {
                    tempStaffs = tempStaffs.OrderBy(x => x.StaffCode);
                }

                //// Check dropdown
                //if (parameters.IsDropdown)
                //{
                //    return
                //}

                return Ok(tempStaffs);

            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel()
                {
                    Code = 1,
                    IsSuccess = false,
                    Message = $"Co loi xay ra {ex.Message}"
                });
            }
        }

        
    }
}
