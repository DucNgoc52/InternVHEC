using IMS_LEARN.Infratructure;
using IMS_LEARN.Models;

namespace IMS_LEARN.Services.SvPermits
{
    public interface IPermitService
    {
        public IQueryable<PermitModel> GetList();
        public PermitModel GetByCode(string code);
        public Permit Create(PermitRequestModel input);
        public Permit Approve(ApproveModel input);
        public Permit Update(PermitModel input);
        public Permit Delete(string permitcode);
    }
}
