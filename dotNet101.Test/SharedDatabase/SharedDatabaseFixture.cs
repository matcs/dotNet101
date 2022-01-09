using dotNet101.Data;
using dotNet101.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet101.UnitTest.SharedDatabase
{
    public class SharedDatabaseFixture : IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        public SharedDatabaseFixture()
        {
            Connection = 
                new SqlConnection("Server=.\\SQLExpress ;Password=test123; User ID=sa;Initial Catalog=StudentsDotNet101_TestDB;Data Source=DDEV-MATHEUSS; Integrated Security=SSPI;");

            Seed();

            Connection.Open();
        }

        public DbConnection Connection { get; }

        public ApplicationDbContext CreateContext(DbTransaction transaction = null)
        {
            var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        var one = new Student() { Name = "Itadori", Grade = "A" };

                        var two = new Student() { Name = "Nobara", Grade = "A" };

                        var three = new Student() { Name = "Megumi", Grade = "A" };
                       
                        context.AddRange(one, two, three);

                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() { Connection.Dispose(); }
    }
}
