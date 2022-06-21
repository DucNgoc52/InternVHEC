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
using IMS_LEARN.Models.Paging;

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

        /// <summary>
        /// Get List Staff by Paging or All
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        public IActionResult GetList(ResquestParameters parameters)
        {
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

                // Check dropdown
                if (parameters.IsDropdown)
                {
                    var tempStaffsDropdown = PagedList<StaffModel>.ToPagedList(tempStaffs.ToList(), 0, tempStaffs.Count());
                    return Ok(new StaffListModel { Items = tempStaffsDropdown });
                }

                int totolCount = tempStaffs.Count();
                //int skip = parameters.Skip != null ? parameters.Skip.Value : 0;
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = tempStaffs.Skip(skip).Take(top).ToList();
                var results = new PagedList<StaffModel>(items, totolCount, (skip / top) + 1, top);

                return Ok(new StaffListModel { Items = results, MetaData = results.MetaData });

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

        /// <summary>
        /// Get Staff by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByCode/{code}")]
        public IActionResult GetByCode(string code)
        {
            try
            {
                var data = _staffService.GetByCode(code);
                return Ok(data);
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

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(StaffModel input)
        {
            try
            {
                var existStaff = _staffService.GetByCode(input.StaffCode);
                if (existStaff == null)
                {
                    var staff = _staffService.Create(input);
                    if (staff != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Tạo mới Staff thành công"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Tạo mới Staff không thành công."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Staff code is exist."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Tạo mới không thành công {ex.Message.ToString()}"
                });
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(StaffModel input)
        {
            try
            {
                var existStaff = _staffService.GetByCode(input.StaffCode);
                if (existStaff != null)
                {
                    var staff = _staffService.Update(input);
                    if (staff != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Update Staff thành công"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Update Staff không thành công."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Staff code không tồn tại."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Update không thành công {ex.Message.ToString()}"
                });
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(string staffcode)
        {
            try
            {
                var existStaff = _staffService.GetByCode(staffcode);
                if (existStaff != null)
                {
                    var staff = _staffService.Delete(staffcode);
                    if (staff != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Xóa Staff thành công"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Xóa Staff không thành công."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Staff code không tồn tại"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Xóa Staff không thành công {ex.Message.ToString()}"
                });
            }
        }
    }
}
