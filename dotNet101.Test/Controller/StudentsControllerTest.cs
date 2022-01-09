using dotNet101.Model;
using Xunit;
using dotNet101.Unit.Test.SharedDatabase;
using dotNet101.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace dotNet101.Unit.Test.Controller
{
    [Trait("CATEGORY","Controller Crud")]
    public class StudentsControllerTest : IClassFixture<SharedDatabaseFixture>
    {
        public StudentsControllerTest(SharedDatabaseFixture fixture) => Fixture = fixture;

        public SharedDatabaseFixture Fixture { get; }

        [Fact]
        public async void GetStudentWithIdEquals1()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var controller = new StudentsController(context);

                    var student = await controller.GetStudent(1).ConfigureAwait(true);

                    Assert.Equal("Itadori", student.Value.Name);
                }
            }
        }

        [Fact]
        public async void PostNewStudent()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var controller = new StudentsController(context);

                    var newStudent = await controller.PostStudent(new Student() { Name = "Yuta", Grade = "Special" });
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var controller = new StudentsController(context);

                    var students = await controller.GetStudents().ConfigureAwait(false);

                    Assert.Equal(4, students.Value.Count());

                    Assert.Equal("Yuta", students.Value.Last().Name);
                }
            }
        }

        [Fact]
        public async void UpdateStudentGradeToSpecial()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var controller = new StudentsController(context);

                    await controller.PutStudent(1, new Student() { StudentId = 1 ,Name = "Itadori", Grade = "Special" });
                    
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var controller = new StudentsController(context);

                    var student = await controller.GetStudent(1).ConfigureAwait(false);

                    Assert.Equal("Special", student.Value.Grade);
                }
            }
        }

        [Fact]
        public async void DeleteStudent()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                using (var context = Fixture.CreateContext(transaction))
                {
                    var controller = new StudentsController(context);

                    await controller.DeleteStudent(1);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var controller = new StudentsController(context);

                    var student = await controller.GetStudent(1);

                    Assert.IsType<NotFoundResult>(student.Result);
                }
            }
        }

    }
}
