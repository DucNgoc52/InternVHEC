using System.ComponentModel.DataAnnotations;

namespace IMS_LEARN.Infratructure
{
    public class Permit
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string PermitCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
        public string Duration { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }
}
