using IMS_LEARN.Infratructure;
using IMS_LEARN.Models;
using IMS_LEARN.Models.StaffModels;

namespace IMS_LEARN.Services.SvStaff
{
    public interface IStaffService
    {
        public IQueryable<StaffModel> GetList();
        public StaffModel GetByCode(string code);
        public Staff Create(StaffModel input);
        public Staff Update(StaffModel input);
        public Staff Delete(string staffcode);
        public Staff ChangePass(ChangePassword request);

        public Staff ForgotPass(ForgotPasswordModel forgot);
        public Staff ResetPass(ResetPassword input);

        public IQueryable<StaffModel> Timkiem(string name);
    }
}
