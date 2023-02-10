using IMS_LEARN.Infratructure;
using IMS_LEARN.Models;
using IMS_LEARN.Services.Base;

namespace IMS_LEARN.Services.SvPermits
{
    public class PermitService : IPermitService
    {
        private readonly IBaseRepository<Permit> _permitService;
        public PermitService(IBaseRepository<Permit> permitService)
        {
            _permitService = permitService;
        }

        public IQueryable<PermitModel> GetList()
        {
            return _permitService.GetAllQueryable().Select(x => new PermitModel()
            {
                Id = x.Id,
                PermitCode = x.PermitCode,
                UserName = x.UserName,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                LeaveType = x.LeaveType,
                Reason = x.Reason,
                Duration = x.Duration,
                Status = x.Status,
                Note = x.Note,
            }).AsQueryable();
        }
        public PermitModel GetByCode(string code)
        {
            return _permitService.GetAllQueryable(x => x.PermitCode.ToLower().Equals(code.ToLower())).Select(x => new PermitModel()
            {
                Id = x.Id,
                PermitCode = x.PermitCode,
                UserName = x.UserName,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                LeaveType = x.LeaveType,
                Reason = x.Reason,
                Duration = x.Duration,
                Status = x.Status,
                Note = x.Note,
            }).FirstOrDefault();
        }

        public Permit Create(PermitRequestModel input)
        {
            // Mapper Library

            Permit permit = new Permit()
            {
                Id = Guid.NewGuid(),
                PermitCode = input.PermitCode,
                UserName = input.UserName,
                FromDate = input.FromDate,
                ToDate = input.ToDate,
                LeaveType = input.LeaveType,
                Reason = input.Reason,
                Duration=input.Duration,
                Status= "Inprocess",
            };

            return _permitService.Insert(permit);
        }

        public Permit Update(PermitModel input)
        {
            var existPermit = _permitService.GetAllQueryable(x => x.PermitCode.ToLower().Equals(input.PermitCode.ToLower())).FirstOrDefault();
            existPermit.UserName = input.UserName;
            existPermit.FromDate = input.FromDate;
            existPermit.ToDate = input.ToDate;
            existPermit.LeaveType = input.LeaveType;
            existPermit.Reason = input.Reason;
            existPermit.Duration = input.Duration;
            existPermit.Status = input.Status;
            existPermit.Note = input.Note;

            return _permitService.Update(existPermit);
        }

        public Permit Delete(string permitcode)
        {
            var existPermit = _permitService.GetAllQueryable(x => x.PermitCode.ToLower().Equals(permitcode.ToLower())).FirstOrDefault();
            return _permitService.Delete(existPermit);
        }

        public Permit Approve(ApproveModel input)
        {
            var existPermit = _permitService.GetAllQueryable(x => x.PermitCode.ToLower().Equals(input.PermitCode.ToLower())).FirstOrDefault();
            existPermit.Status = input.Status;
            existPermit.Note = input.Note;

            return _permitService.Update(existPermit);
        }
    }
}
