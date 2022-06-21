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

        public Staff Create(StaffModel input)
        {
            // Mapper Library

            Staff staff = new Staff()
            {
                Id = Guid.NewGuid(),
                StaffCode = input.StaffCode,
                FirtName = input.FirtName,
                MidleName = input.MidleName,
                LastName = input.LastName,
                Status = input.Status,
                DateOn = input.DateOn
            };

            return _staffService.Insert(staff);
        }

        public Staff Update(StaffModel input)
        {
            var existStaff = _staffService.GetAllQueryable(x => x.StaffCode.ToLower().Equals(input.StaffCode.ToLower())).FirstOrDefault();
            existStaff.FirtName = input.FirtName;
            existStaff.MidleName = input.MidleName;
            existStaff.LastName = input.LastName;
            existStaff.Status = input.Status;
            existStaff.DateOn = input.DateOn;

            return _staffService.Update(existStaff);
        }

        public Staff Delete(string staffcode)
        {
            var existStaff = _staffService.GetAllQueryable(x => x.StaffCode.ToLower().Equals(staffcode.ToLower())).FirstOrDefault();
            return _staffService.Delete(existStaff);
        }
    }
}
