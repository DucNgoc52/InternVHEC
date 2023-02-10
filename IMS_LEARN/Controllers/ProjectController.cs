using IMS_LEARN.Common;
using IMS_LEARN.Models;
using IMS_LEARN.Services.SvProject;
using IMS_LEARN.Models.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Linq.Dynamic;
using Microsoft.AspNetCore.Authorization;

namespace IMS_LEARN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpPost]
        [Route("GetList")]
        public IActionResult GetList(ResquestParameters parameters)
        {
            try
            {
                var tempProject = _projectService.GetList();

                // Search
                if (!string.IsNullOrEmpty(parameters.Filter))
                {
                    var optionAssembly = ScriptOptions.Default.AddReferences(typeof(ProjectModel).Assembly);
                    var tempFilterExpression = CSharpScript.EvaluateAsync<Func<ProjectModel, bool>>($"s=> {parameters.Filter}", optionAssembly);
                    Func<ProjectModel, bool> filterExpression = tempFilterExpression.Result;

                    tempProject = tempProject.Where(filterExpression).AsQueryable();
                }

                // Order by
                if (!string.IsNullOrEmpty(parameters.OrderBy))
                {
                    tempProject = tempProject.OrderBy(parameters.OrderBy);
                }
                else
                {
                    tempProject = tempProject.OrderBy(x => x.PrjCode);
                }

                // Check dropdown
                if (parameters.IsDropdown)
                {
                    var tempProjectsDropdown = PagedList<ProjectModel>.ToPagedList(tempProject.ToList(), 0, tempProject.Count());
                    return Ok(new ProjectListModel { Items = tempProjectsDropdown });
                }

                int totolCount = tempProject.Count();
                //int skip = parameters.Skip != null ? parameters.Skip.Value : 0;
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = tempProject.Skip(skip).Take(top).ToList();
                var results = new PagedList<ProjectModel>(items, totolCount, (skip / top) + 1, top);

                return Ok(new ProjectListModel { Items = results, MetaData = results.MetaData });

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
                var data1 = _projectService.GetByCode(code);
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
        public IActionResult Create(ProjectModel input)
        {
            try
            {
                var existProject = _projectService.GetByCode(input.PrjCode);
                if (existProject == null)
                {
                    var project = _projectService.Create(input);
                    if (project != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Add Project successfully"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Add Project failure."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Project is exist!"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Add Project failure. {ex.Message.ToString()}"
                });
            }
        }


        [HttpPut]
        [Route("Update")]
        public IActionResult Update(ProjectModel input)
        {
            try
            {
                var existProject = _projectService.GetByCode(input.PrjCode);
                if (existProject != null)
                {
                    var project = _projectService.Update(input);
                    if (project != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Update Project successfully"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Update Project failure!"
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Project Code is exist"
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
        public IActionResult Delete(string maduan)
        {
            try
            {
                var existProject = _projectService.GetByCode(maduan);
                if (existProject != null)
                {
                    var staff = _projectService.Delete(maduan);
                    if (staff != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Delete Project successfully"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Delete Project failure."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Project Code is not exist"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Delete Project failure {ex.Message.ToString()}"
                });
            }
        }
    }
}
