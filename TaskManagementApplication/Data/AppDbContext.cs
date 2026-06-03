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
    }
}
