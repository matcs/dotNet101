using dotNet101.Model;
using dotNet101.Service;
using Xunit;
using dotNet101.UnitTest.SharedDatabase;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace dotNet101.UnitTest.Services
{
    public class StudentServiceTest : IClassFixture<SharedDatabaseFixture>
    {
        public StudentServiceTest(SharedDatabaseFixture fixture) => Fixture = fixture;

        private SharedDatabaseFixture Fixture { get; }

        [Fact]
        public async void TestGetStudentByIdReturnsStudent()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);

                    var student = await service.GetStudentById(1).ConfigureAwait(true);

                    Assert.Equal("Itadori", student.Name);
                }
            }
        }

         [Fact]
         public async void TestGetStudentByIdReturnsNull()
         {
             using (var transaction = Fixture.Connection.BeginTransaction())
             {
                 using (var context = Fixture.CreateContext(transaction))
                 {
                     var service = new StudentsService(context);
        
                     var student = await service.GetStudentById(4).ConfigureAwait(true);
        
                     Assert.Null(student);
                 }
             }
        }
        
        [Fact]
        public async void TestAddNewStudentThenTheCountTotalOfStudentsShouldBe4()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);

                    var newStudent = await service.AddStudent(new Student() { Name = "Yuta", Grade = "Special" });
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);

                    var students = await service.GetAllStudents().ConfigureAwait(false);

                    Assert.Equal(4, students.Count);

                    Assert.Equal("Yuta", students.Last().Name);
                }
            }
        }

        [Fact]
        public async void TestAddNewStudentWithMissingNameShouldNotAdd()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);

                    var newStudent = await service.AddStudent(new Student() { Name = "", Grade = "Special" });
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);

                    var students = await service.GetAllStudents().ConfigureAwait(false);

                    Assert.Equal(3, students.Count);

                    Assert.Equal("Megumi", students.Last().Name);
                }
            }
        }

        [Fact]
        public async void TestUpdateStudentThenReturnNull()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);
                    var student = await service.UpdateStudent(new Student() { StudentId = 5, Name = "Itadori", Grade = "Special" });

                    Assert.Null(student);
                }
            }
        }

        [Fact]
        public async void TestUpdateStudentGradeToSpecial()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);

                    await service.UpdateStudent(new Student() { StudentId = 1, Name = "Itadori", Grade = "Special" });
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var controller = new StudentsService(context);

                    var student = await controller.GetStudentById(1).ConfigureAwait(false);

                    Assert.Equal("Special", student.Grade);
                }
            }
        }

        [Fact]
        public async void TestDeleteStudentReturnStudentThenSearchForTheStudentAndReturnNull()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);

                    var student = await service.DeleteStudent(1);

                    Assert.IsType<EntityEntry<Student>>(student);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);

                    var student = await service.GetStudentById(1);

                    Assert.Null(student);
                }
            }
        }

        [Fact]
        public async void TestDeleteStudentReturnNull()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var service = new StudentsService(context);

                    var student = await service.DeleteStudent(5);

                    Assert.Null(student);
                }
            }
        }

    }
}
