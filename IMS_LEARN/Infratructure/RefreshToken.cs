using System.ComponentModel.DataAnnotations;

namespace IMS_LEARN.Infratructure
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public string StaffCode { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
