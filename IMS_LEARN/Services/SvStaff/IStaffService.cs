using IMS_LEARN.Infratructure;
using IMS_LEARN.Models;

namespace IMS_LEARN.Services.SvStaff
{
    public interface IStaffService
    {
        public IQueryable<StaffModel> GetList();
        public StaffModel GetByCode(string code);
        public Staff Create(StaffModel input);
        public Staff Update(StaffModel input);
        public Staff Delete(string staffcode);
    }
}
