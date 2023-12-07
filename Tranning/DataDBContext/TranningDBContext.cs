using Microsoft.EntityFrameworkCore;

namespace Tranning.DataDBContext
{
    public class TranningDBContext : DbContext
    {
        public TranningDBContext(DbContextOptions<TranningDBContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Topic> Topic { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.category_id);
        }
    }
}
