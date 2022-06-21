using IMS_LEARN.Models;
using IMS_LEARN.Infratructure;
using IMS_LEARN.Services.Base;

namespace IMS_LEARN.Services.SvStaff
{
    public class StaffService : IStaffService
    {
        private readonly IBaseRepository<Staff> _staffService;

        public StaffService(IBaseRepository<Staff> staffService)
        {
            _staffService = staffService;
        }

        public IQueryable<StaffModel> GetList()
        {
            return _staffService.GetAllQueryable().Select(x => new StaffModel()
            {
                Id = x.Id,
                StaffCode = x.StaffCode,
                FirtName = x.FirtName,
                MidleName = x.MidleName,
                LastName = x.LastName,
                Status = x.Status,
                DateOn = x.DateOn
            }).AsQueryable();
        }

        public StaffModel GetByCode(string code)
        {
            return _staffService.GetAllQueryable(x=>x.StaffCode.ToLower().Equals(code.ToLower())).Select(x => new StaffModel()
            {
                Id = x.Id,
                StaffCode = x.StaffCode,
                FirtName = x.FirtName,
                MidleName = x.MidleName,
                LastName = x.LastName,
                Status = x.Status,
                DateOn = x.DateOn
            }).FirstOrDefault();
        }
    }
}
