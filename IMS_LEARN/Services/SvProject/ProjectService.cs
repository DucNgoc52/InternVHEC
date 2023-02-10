using IMS_LEARN.Infratructure;
using IMS_LEARN.Models;
using IMS_LEARN.Services.Base;

namespace IMS_LEARN.Services.SvProject
{
    public class ProjectService : IProjectService
    {
        private readonly IBaseRepository<Project> _projectService;
        public ProjectService(IBaseRepository<Project> projectService)
        {
            _projectService = projectService;
        }

        public IQueryable<ProjectModel> GetList()
        {
            return _projectService.GetAllQueryable().Select(x => new ProjectModel()
            {
                Id = x.Id,
                PrjCode = x.PrjCode,
                PrjName = x.PrjName,
                RqDate = x.RqDate,
                Custom = x.Custom,
                Deparment = x.Deparment,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Paymonth = x.Paymonth,
                Status = x.Status,
                Remark = x.Remark,
                Update = x.Update,
                UserUpdate = x.UserUpdate,
                CreatDate = x.CreatDate
            }).AsQueryable();
        }

        public ProjectModel GetByCode(string code)
        {
            return _projectService.GetAllQueryable(x => x.PrjCode.ToLower().Equals(code.ToLower())).Select(x => new ProjectModel()
            {
                Id = x.Id,
                PrjCode = x.PrjCode,
                PrjName = x.PrjName,
                RqDate = x.RqDate,
                Custom = x.Custom,
                Deparment = x.Deparment,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Paymonth = x.Paymonth,
                Status = x.Status,
                Remark = x.Remark,
                Update = x.Update,
                UserUpdate = x.UserUpdate,
                CreatDate = x.CreatDate
            }).FirstOrDefault();
        }
        public Project Create(ProjectModel input)
        {
            // Mapper Library

            Project project = new Project()
            {
                Id = Guid.NewGuid(),
                PrjCode = input.PrjCode,
                PrjName = input.PrjName,
                RqDate = input.RqDate,
                Custom = input.Custom,
                Deparment = input.Deparment,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                Paymonth = input.Paymonth,
                Status = input.Status,
                Remark = input.Remark,
                Update = input.Update,
                UserUpdate = input.UserUpdate,
                CreatDate = input.CreatDate
            };

            return _projectService.Insert(project);
        }
        public Project Update(ProjectModel input)
        {
            var existProject = _projectService.GetAllQueryable(x => x.PrjCode.ToLower().Equals(input.PrjCode.ToLower())).FirstOrDefault();
            existProject.PrjName = input.PrjName;
            existProject.RqDate = input.RqDate;
            existProject.Custom = input.Custom;
            existProject.Deparment = input.Deparment;
            existProject.StartDate = input.StartDate;
            existProject.EndDate = input.EndDate;
            existProject.Paymonth = input.Paymonth;
            existProject.Status = input.Status;
            existProject.Remark = input.Remark;
            existProject.Update = input.Update;
            existProject.UserUpdate = input.UserUpdate;
            existProject.CreatDate = input.CreatDate;

            return _projectService.Update(existProject);
        }
        public Project Delete(string maduan)
        {
            var existProject = _projectService.GetAllQueryable(x => x.PrjCode.ToLower().Equals(maduan.ToLower())).FirstOrDefault();
            return _projectService.Delete(existProject);
        }
    }
}
