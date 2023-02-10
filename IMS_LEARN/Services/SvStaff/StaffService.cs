using IMS_LEARN.Models;
using IMS_LEARN.Infratructure;
using IMS_LEARN.Services.Base;
using System.Security.Cryptography;
using IMS_LEARN.Models.StaffModels;

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
                UserName = x.UserName,
                PassWord = x.PassWord,
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
                UserName= x.UserName,
                PassWord= x.PassWord,
                FirtName = x.FirtName,
                MidleName = x.MidleName,
                LastName = x.LastName,
                Status = x.Status,
                DateOn = x.DateOn,
                Email = x.Email
            }).FirstOrDefault();
        }

        public static string CreateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes);
            }
        }

        public Staff Create(StaffModel input)
        {
            Staff staff = new Staff()
            {
                Id = Guid.NewGuid(),
                StaffCode = input.StaffCode,
                FirtName = input.FirtName,
                MidleName = input.MidleName,
                LastName = input.LastName,
                Status = input.Status,
                DateOn = input.DateOn,
                UserName = input.UserName,
                PassWord = CreateMD5(input.PassWord),
                Email = input.Email
            };

            return _staffService.Insert(staff);
        }

        public Staff Update(StaffModel input)
        {
            var existStaff = _staffService.GetAllQueryable(x => x.StaffCode.ToLower().Equals(input.StaffCode.ToLower())).FirstOrDefault();
            existStaff.UserName = input.UserName;
            existStaff.PassWord = CreateMD5(input.PassWord);
            existStaff.FirtName = input.FirtName;
            existStaff.MidleName = input.MidleName;
            existStaff.LastName = input.LastName;
            existStaff.Status = input.Status;
            existStaff.DateOn = input.DateOn;
            existStaff.Email = input.Email;

            return _staffService.Update(existStaff);
        }

        public Staff Delete(string staffcode)
        {
            var existStaff = _staffService.GetAllQueryable(x => x.StaffCode.ToLower().Equals(staffcode.ToLower())).FirstOrDefault();
            return _staffService.Delete(existStaff);
        }

        public Staff ChangePass(ChangePassword request)
        {
            var pass = _staffService.GetAllQueryable(x => x.PassWord.ToLower().Equals(CreateMD5(request.CurrentPassword).ToLower())).FirstOrDefault();
            pass.PassWord = CreateMD5(request.NewPassword);
            return _staffService.Update(pass);
        }
        public Staff ForgotPass(ForgotPasswordModel forgot)
        {
            var user = _staffService.GetAllQueryable(x => x.Email.Equals(forgot.Email)).FirstOrDefault();
            user.ResetPasswordCode = Guid.NewGuid().ToString();
            return _staffService.Update(user);
        }

        public Staff ResetPass(ResetPassword input)
        {
            var user = _staffService.GetAllQueryable(x => x.ResetPasswordCode.Equals(input.TokenResetPassword)).FirstOrDefault();
            user.PassWord = CreateMD5(input.NewPassword);
            return _staffService.Update(user);
        }

        public IQueryable<StaffModel> Timkiem(string name)
        {
            return _staffService.GetAllQueryable(x => x.LastName.ToLower().Contains(name.ToLower())).Select(x => new StaffModel()
            {
                Id = x.Id,
                StaffCode = x.StaffCode,
                UserName = x.UserName,
                PassWord = x.PassWord,
                FirtName = x.FirtName,
                MidleName = x.MidleName,
                LastName = x.LastName,
                Status = x.Status,
                DateOn = x.DateOn,
                Email = x.Email
            }).AsQueryable();
        }
    }
}
