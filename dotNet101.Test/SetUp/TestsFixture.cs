using dotNet101.Data;
using dotNet101.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet101.Test.SetUp
{
    public class TestsFixture : IDisposable
    {
        public DbContextOptions<ApplicationDbContext> optionsBuilder { get; private set; }

        public TestsFixture()
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "database_name").Options;
            using (var context = new ApplicationDbContext(optionsBuilder))
            {
                context.Students.Add(new Student() { StudentId = 1, Name = "Title 1" });
                context.Students.Add(new Student() { StudentId = 2, Name = "Title 2" });
                context.Students.Add(new Student() { StudentId = 3, Name = "Title 3" });
                context.SaveChanges();
            }
        }

        public void Dispose() { }
    }
}
