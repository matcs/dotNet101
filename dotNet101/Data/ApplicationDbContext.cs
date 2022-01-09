using dotNet101.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
