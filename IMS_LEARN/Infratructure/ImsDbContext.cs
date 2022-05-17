using Microsoft.EntityFrameworkCore;

namespace IMS_LEARN.Infratructure
{
    public partial class ImsDbContext : DbContext
    {
        public ImsDbContext()
        {

        }

        public ImsDbContext(DbContextOptions<ImsDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Staff> Staffs { get; set; }
    }
}
