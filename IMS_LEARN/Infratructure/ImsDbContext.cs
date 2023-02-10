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
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Permit> Permits { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
            });
        }
    }
}
