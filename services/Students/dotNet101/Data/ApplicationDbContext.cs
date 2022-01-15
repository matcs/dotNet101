using dotNet101.Model;
using Microsoft.EntityFrameworkCore;

namespace dotNet101.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                       .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=StudentsDotNet101_DB;Trusted_Connection=True;",
                       providerOptions => providerOptions.CommandTimeout(100)); 
            }
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public ApplicationDbContext() : base() { }

        public DbSet<Student> Students { get; set; }
    }
}
