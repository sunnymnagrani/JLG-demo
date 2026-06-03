using Microsoft.EntityFrameworkCore;
using TaskManagementApplication.Models;

namespace TaskManagementApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<TaskData> TaskList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Sets PostgreSQL system time as the default value on creation
            modelBuilder.Entity<TaskData>()
                .Property(t => t.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<TaskData>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.UpdatedDate = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
