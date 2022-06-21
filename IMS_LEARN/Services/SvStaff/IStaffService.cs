using IMS_LEARN.Models;

namespace IMS_LEARN.Services.SvStaff
{
    public interface IStaffService
    {
        public IQueryable<StaffModel> GetList();
        public StaffModel GetByCode(string code);
    }
}
