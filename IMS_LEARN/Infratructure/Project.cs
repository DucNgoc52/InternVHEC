using System.ComponentModel.DataAnnotations;

namespace IMS_LEARN.Infratructure
{
    public class Project
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string PrjCode { get; set; }
        [Required]
        [MaxLength(20)]
        public string PrjName { get; set; }
        public DateTime RqDate { get; set; }
        [Required]
        public string Custom { get; set; }
        public string Deparment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Paymonth { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string Update { get; set; }
        public string UserUpdate { get; set; }
        public DateTime CreatDate { get; set; }
    }
}
