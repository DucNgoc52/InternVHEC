using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_LEARN.Infratructure
{
    public class Staff
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string StaffCode { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }

        [MaxLength(50)]
        public string FirtName { get; set; }
        [MaxLength(50)]
        public string MidleName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ResetPasswordCode { get; set; }
        public string Status { get; set; }
        public DateTime DateOn { get; set; }
    }
}
