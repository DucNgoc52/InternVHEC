using IMS_LEARN.Common;
using IMS_LEARN.Models;
using IMS_LEARN.Models.Paging;
using IMS_LEARN.Services.SvPermits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Linq;
using System.Linq.Dynamic;

namespace IMS_LEARN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PermitController : Controller
    {
        private readonly IPermitService _permitService;
        public PermitController(IPermitService permitService)
        {
            _permitService = permitService;
        }
        [HttpPost]
        [Route("GetList")]
        public IActionResult GetList(ResquestParameters parameters)
        {
            try
            {
                var tempPermit = _permitService.GetList();

                // Search
                if (!string.IsNullOrEmpty(parameters.Filter))
                {
                    var optionAssembly = ScriptOptions.Default.AddReferences(typeof(PermitModel).Assembly);
                    var tempFilterExpression = CSharpScript.EvaluateAsync<Func<PermitModel, bool>>($"s=> {parameters.Filter}", optionAssembly);
                    Func<PermitModel, bool> filterExpression = tempFilterExpression.Result;

                    tempPermit = tempPermit.Where(filterExpression).AsQueryable();
                }

                // Order by
                if (!string.IsNullOrEmpty(parameters.OrderBy))
                {
                    tempPermit = tempPermit.OrderBy(parameters.OrderBy);
                }
                else
                {
                    tempPermit = tempPermit.OrderBy(x => x.PermitCode);
                }

                // Check dropdown
                if (parameters.IsDropdown)
                {
                    var tempPermitDropdown = PagedList<PermitModel>.ToPagedList(tempPermit.ToList(), 0, tempPermit.Count());
                    return Ok(new PermitListModel1 { Items = tempPermitDropdown });
                }

                int totolCount = tempPermit.Count();
                //int skip = parameters.Skip != null ? parameters.Skip.Value : 0;
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = tempPermit.Skip(skip).Take(top).ToList();
                var results = new PagedList<PermitModel>(items, totolCount, (skip / top) + 1, top);

                return Ok(new PermitListModel1 { Items = results, MetaData = results.MetaData });

            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel()
                {
                    Code = 1,
                    IsSuccess = false,
                    Message = $"Something went wrong! {ex.Message}"
                });
            }
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public IActionResult GetByCode(string code)
        {
            try
            {
                var data1 = _permitService.GetByCode(code);
                return Ok(data1);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel()
                {
                    Code = 1,
                    IsSuccess = false,
                    Message = $"Something went wrong! {ex.Message}"
                });
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(PermitRequestModel input)
        {
            try
            {
                var existPermit = _permitService.GetByCode(input.PermitCode);
                if (existPermit == null)
                {
                    var permit = _permitService.Create(input);
                    if (permit != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Add Permit successfully"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Add Permit failure."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Permit is exist."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Add Permit failure. {ex.Message.ToString()}"
                });
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(PermitModel input)
        {
            try
            {
                var existPermit = _permitService.GetByCode(input.PermitCode);
                if (existPermit != null)
                {
                    var permit = _permitService.Update(input);
                    if (permit != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Update Permit successfully"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Update Permit failure."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Permit Code is not exits!"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Update failure {ex.Message.ToString()}"
                });
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(string permitcode)
        {
            try
            {
                var existPermit = _permitService.GetByCode(permitcode);
                if (existPermit != null)
                {
                    var permit = _permitService.Delete(permitcode);
                    if (permit != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Delete Permit successfully"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Delete Permit failure."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Permit Code is not exits!"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Delete Permit failure. {ex.Message.ToString()}"
                });
            }
        }

        [HttpPut]
        [Route("Approve")]
        public IActionResult Approve(ApproveModel input)
        {
            try
            {
                var existPermit = _permitService.GetByCode(input.PermitCode);
                if (existPermit != null)
                {
                    var permit = _permitService.Approve(input);
                    if (permit != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Approve successfully"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Approve failure."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Permit Code is not exits!"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Approve failure. {ex.Message.ToString()}"
                });
            }
        }
    }
}
