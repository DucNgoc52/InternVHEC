using System.ComponentModel.DataAnnotations;

namespace IMS_LEARN.Infratructure
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string StaffCode { get; set; }
        [MaxLength(50)]
        public string FirtName { get; set; }
        [MaxLength(50)]
        public string MidleName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public string Status { get; set; }
        public DateTime DateOn { get; set; }
    }
}
