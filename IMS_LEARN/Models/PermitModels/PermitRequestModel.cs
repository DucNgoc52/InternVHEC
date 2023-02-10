namespace IMS_LEARN.Models
{
    public class PermitRequestModel
    {
        public Guid Id { get; set; }
        public string PermitCode { get; set; }
        public string UserName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
        public string Duration { get; set; }
    }
}
