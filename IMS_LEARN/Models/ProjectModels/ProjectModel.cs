using IMS_LEARN.Common;

namespace IMS_LEARN.Models
{
    public class ProjectModel
    {
        public Guid Id { get; set; }
        public string PrjCode { get; set; }
        public string PrjName { get; set; }
        public DateTime RqDate { get; set; }
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
    public class ProjectListModel
    {
        public List<ProjectModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
