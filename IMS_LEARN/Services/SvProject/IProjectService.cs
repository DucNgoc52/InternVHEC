using IMS_LEARN.Infratructure;
using IMS_LEARN.Models;

namespace IMS_LEARN.Services.SvProject
{
    public interface IProjectService
    {
        public IQueryable<ProjectModel> GetList();
        public ProjectModel GetByCode(string code);
        public Project Create(ProjectModel input);
        public Project Update(ProjectModel input);
        public Project Delete(string maduan);
    }
}
