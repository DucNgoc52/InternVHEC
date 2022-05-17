namespace IMS_LEARN.Models
{
    public class StaffModel
    {
        public int Id { get; set; }
        public string StaffCode { get; set; }
        public string FirtName { get; set; }
        public string MidleName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public DateTime DateOn { get; set; }
    }

    public class CreateStaffRequest
    {

    }
}
