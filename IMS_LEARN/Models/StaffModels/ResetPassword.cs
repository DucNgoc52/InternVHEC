namespace IMS_LEARN.Models
{
    public class ResetPassword
    {
        public string TokenResetPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
