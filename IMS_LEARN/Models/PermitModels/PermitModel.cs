using IMS_LEARN.Common;

namespace IMS_LEARN.Models
{
    public class PermitModel
    {
        public Guid Id { get; set; }
        public string PermitCode { get; set; }
        public string UserName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
        public string Duration { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }
    public class PermitListModel1
    {
        public List<PermitModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
