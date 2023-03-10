using IMS_LEARN.Common;

namespace IMS_LEARN.Models
{
    public class StaffModel
    {
        public Guid Id { get; set; }
        public string StaffCode { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string FirtName { get; set; }
        public string MidleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public DateTime DateOn { get; set; }
    }

    public class StaffListModel
    {
        public List<StaffModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
